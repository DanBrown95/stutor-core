using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.BillingPortal;
using stutor_core.Models.ViewModels;
using stutor_core.Services.Interfaces;

namespace stutor_core.Controllers
{
    [Route("create-payment-intent")]
    [Authorize]
    [ApiController]
    public class PaymentIntentApiController : Controller
    {
        IExpertService _expertService;
        private readonly decimal serviceFee = 2.50M;

        public PaymentIntentApiController(IExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] PaymentIntentCreateRequest purchase)
        {
            //var paymentIntents = new PaymentIntentService();
            //var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            //{
            //    Amount = CalculateOrderAmount(purchase.ExpertId, purchase.TopicId),
            //    Currency = "usd",
            //});
            //return Json(new { clientSecret = paymentIntent.ClientSecret });

            // Alternatively, set up a webhook to listen for the payment_intent.succeeded event
            // and attach the PaymentMethod to a new Customer
            var customers = new CustomerService();
            var customer = customers.Create(new CustomerCreateOptions());
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Customer = customer.Id,
                SetupFutureUsage = "off_session",
                Amount = CalculateOrderAmount(purchase.ExpertId, purchase.TopicId),
                Currency = "usd",
                CaptureMethod = "manual"
            });
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        //public void ChargeCustomer(string customerId)
        //{
        //    // Lookup the payment methods available for the customer
        //    var paymentMethods = new PaymentMethodService();
        //    var availableMethods = paymentMethods.List(new PaymentMethodListOptions
        //    {
        //        Customer = customerId,
        //        Type = "card",
        //    });
        //    // Charge the customer and payment method immediately
        //    var paymentIntents = new PaymentIntentService();
        //    var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
        //    {
        //        Amount = 1099,
        //        Currency = "usd",
        //        Customer = customerId,
        //        PaymentMethod = availableMethods.Data[0].Id,
        //        OffSession = true,
        //        Confirm = true
        //    });
        //    if (paymentIntent.Status == "succeeded")
        //        Console.WriteLine("✅ Successfully charged card off session");
        //}

        private int CalculateOrderAmount(string expertId, int topicId)
        {
            // Replace this constant with a calculation of the order's amount
            // Calculate the order total on the server to prevent
            // people from directly manipulating the amount on the client

            var expertCost = _expertService.GetExpertPrice(expertId, topicId);
            var result = Convert.ToInt32((expertCost + serviceFee) * 100);
            return result;
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("expertId")]
            public string ExpertId { get; set; }
            [JsonProperty("topicId")]
            public int TopicId { get; set; }
        }
    }

    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class PaymentPortalController : Controller
    {
        IUserService _userService;

        public PaymentPortalController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult RedirectToCustomerPortal([FromBody] StripeCustomerPortalVM model)
        {
            // Authenticate your user. Get customerID by userId from db
            var storedUser = _userService.Get(model.UserId);
            if (model.CustomerId != null || storedUser.CustomerId != null)
            {
                if (model.CustomerId != storedUser.CustomerId)
                {
                    return NotFound();
                }
            }

            if (model.CustomerId == null || model.CustomerId == "")
            {
                //create new customer and store that customer Id
                var customerOptions = new CustomerCreateOptions
                {
                    Email = storedUser.Email,
                    Phone = storedUser.Phone
                };
                var customerService = new CustomerService();
                model.CustomerId = customerService.Create(customerOptions).Id;
                _userService.UpdateStripeCustomerId(model.UserId, model.CustomerId);
            }

            var options = new SessionCreateOptions
            {
                Customer = model.CustomerId,
                ReturnUrl = "http://localhost:8080/",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Json(session.Url);
        }
    }
}