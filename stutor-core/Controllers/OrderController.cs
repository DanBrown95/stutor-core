using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using stutor_core.Configurations;
using stutor_core.Database;
using stutor_core.Models.Enumerations;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Services.Controllers;
using stutor_core.Services.Interfaces;
using stutor_core.Utilities;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SMSSettings _smsSettings;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IExpertService _expertService;
        private readonly IDictionaryService _dictionaryService;

        private readonly decimal serviceFee = 2.50M;

        public OrderController(ApplicationDbContext dbContext, IEmailService emailService, IOptions<SMSSettings> smsSettings, IOrderService orderService, IExpertService expertService, IDictionaryService dictionaryService)
        {
            _dbContext = dbContext;
            _smsSettings = smsSettings.Value;
            _orderService = orderService;
            _emailService = emailService;
            _expertService = expertService;
            _dictionaryService = dictionaryService;
        }

        [HttpPost]
        public int SubmitIntent([FromBody] SubmitIntentVM vm)
        {
            vm.Status = OrderStatus.Unanswered;
            vm.Charge = vm.Price + serviceFee;
            var orderId = _orderService.Create(vm);
            // need to store the order, then use the id to create the OrderPasskey object in the db
            if (orderId > 0)
            {
                var unhashed = _dictionaryService.GetRandomWord();

                var hashed = PasskeySecurity.Hash(unhashed);
                var orderPasskey = new OrderPasskey() { OrderId = orderId, ClientPasskey = hashed };
                var result = _orderService.CreateOrderPasskey(orderPasskey);

                if (result > 0) // Order passkey has been created. Send confirmation text and email
                {
                    var user = _dbContext.User.FirstOrDefault(u => u.Id == vm.UserId);
                    try
                    {
                        _emailService.SendOrderConfirmationEmail(user.Firstname, user.Email, unhashed, vm.FriendlySubmitted, orderId, vm.Price, vm.Charge, serviceFee, vm.TopicName);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to send order confirmation email. {error}", e.Message);
                    }

                    //Send the confirmation text message to the user
                    var smsService = new SmsService(_smsSettings);
                    try
                    {
                        smsService.SendConfirmation(orderId, unhashed, user.Phone);
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to send order confirmation text to ordering user phone {phone}", user.Phone);
                    }

                    
                    // Send the services request email to the expert
                    try
                    {
                        var expert = _expertService.Get(vm.ExpertId);
                        _emailService.SendExpertRequestEmail(expert.User.Email, vm.TopicName);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to send services request email to expert. {error}", ex.Message);
                    }

                    //Send the text message to the expert
                    string expertPhone = "";
                    try
                    {
                        expertPhone = _expertService.GetPhoneById(vm.ExpertId);
                        smsService.SendClientNumber(user.Phone, expertPhone, vm.TopicName);
                    }
                    catch (Exception)
                    {
                        Log.Error("Could not send order text to Expert {expertId} with phone {expertPhone} for user with phone {userPhone}", vm.ExpertId, expertPhone, vm.UserPhone);
                    }
                    CaptureFunds(vm.PaymentIntentId, orderId);
                    return 200;
                }

                Log.Error("Failed to create order passkey for order {order}. {paymentIntentId} ", orderId);
                CancelPayment(vm.PaymentIntentId, orderId);
                return 500;
            }

            Log.Error("Failed to save order to Database for user {userId}. {paymentIntentId} {price} {charge} {topic}", vm.PaymentIntentId, vm.UserId, vm.Price, vm.Charge, vm.TopicName);
            CancelPayment(vm.PaymentIntentId, orderId);
            return 500;
        }

        private bool CaptureFunds(string paymentIntentId, int orderId)
        {
            var service = new Stripe.PaymentIntentService();
            var paymentIntent = service.Capture(paymentIntentId);
            if (paymentIntent.Status != "succeeded")
            {
                Log.Error("CRITICAL: Failed to capture payment from user. Their payment is on hold. Stripe will revert their payment in 7 days unless captured. {paymentIntentId}.", paymentIntentId);
                _orderService.SetRequiresCapture(orderId);
                return false;
            }
            return true;
        }

        private bool CancelPayment(string paymentIntentId, int orderId)
        {
            var service = new Stripe.PaymentIntentService();
            var options = new Stripe.PaymentIntentCancelOptions { };
            var intent = service.Cancel(paymentIntentId, options);
            if (intent == null || intent.Status != "canceled")
            {
                Log.Error("CRITICAL: Failed to cancel payment after server side issue triggered a refund. Payment is still on hold. Take over! {paymentIntentId}, {orderId}.", paymentIntentId, orderId);
                _orderService.UpdateStatus(OrderStatus.CancelationPending, orderId);

                return false;
            }else if(intent != null && intent.Status == "canceled") {
                Log.Error("REMEDIED: Payment intent canceled after server side issue triggered a refund. Verify canceled payment on stripe.com. {paymentIntentId}, {orderId}.", paymentIntentId, orderId);
                _orderService.UpdateStatus(OrderStatus.Canceled, orderId);

                return true;
            }
            Log.Error("CRITICAL: Payment cancellation status unknown after server side issue triggered a refund. Payment may still be on hold. verify payment is canceled on on stripe.com. Take over! {paymentIntentId}, {orderId}.", paymentIntentId, orderId);
            _orderService.UpdateStatus(OrderStatus.CancelationPending, orderId);

            return false;
        }

        [HttpPost]
        public int SubmitPasskeys(SubmitPasskeyVm vm)
        {
            //Check if order has already been validated
            //If not check to see if the passkeys are correct and mark the order as validated.
            var order = _orderService.Get(vm.OrderId);
            if(order.Status != OrderStatus.Unanswered)
            {
                return 0;
            }
            var stored = _orderService.GetOrderPasskey(vm.OrderId).ClientPasskey;
            var result = _orderService.AuthenticatePasskey(vm.OrderId, vm.ClientPasskey, stored);
            return result;
        }

        [HttpPost]
        public JsonResult updateFeedback(Order incomingOrder)
        {
            JsonResult result = Json(new { error = ""});
            try
            {
                var rowsAffected = _orderService.UpdateFeedback(incomingOrder);
                result = (rowsAffected > 0) ? Json(new { status = 200, error = "" }) : Json(new { status = 500, error = "Could not update feedback. Please try again later." });
            }
            catch (System.Exception ex)
            {
                result = Json(new { status = 500, error = ex.Message });
                Log.Error("Failed to update order feedback for order {orderId} with rating {rating} and additional info {additionalInfo}", incomingOrder.Id, incomingOrder.Rating, incomingOrder.AdditionalInfo);
            }
            return result;
        }

        [HttpPost]
        public IEnumerable<Order> GetAllByUserId([FromBody] string userId)
        {   
            return _orderService.GetAllByUserId(userId);
        }
    }
}