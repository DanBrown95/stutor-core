using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using stutor_core.Configurations;
using stutor_core.Database;
using stutor_core.Models;
using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using stutor_core.Repositories;
using stutor_core.Services;
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
        private readonly ApplicationDbContext _db;
        private readonly OrderService _repo;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SMSSettings _smsSettings;
        private readonly ExpertService _expertService;
        private readonly decimal serviceFee = 2.50M;

        public OrderController(ApplicationDbContext db, IEmailService emailService, IHostingEnvironment hostingEnvironment, SMSSettings smsSettings)
        {
            _db = db;
            _repo = new OrderService(_db);
            _emailService = emailService;
            _smsSettings = smsSettings;
            _hostingEnvironment = hostingEnvironment;
            _expertService = new ExpertService(_db);
        }

        [HttpPost]
        public int SubmitIntent([FromBody] SubmitIntentVM vm)
        {
            vm.Status = "Unanswered";
            vm.Charge = vm.Price + serviceFee;
            var orderId = _repo.Create(vm);
            // need to store the order, then use the id to create the OrderPasskey object in the db
            if (orderId > 0)
            {
                var dictionaryRepo = new DictionaryRepository(_db);
                var unhashed = dictionaryRepo.GetRandomWord();

                var hashed = PasskeySecurity.Hash(unhashed);
                var orderPasskey = new OrderPasskey() { OrderId = orderId, ClientPasskey = hashed };
                var result = _repo.CreateOrderPasskey(orderPasskey);

                if (result > 0) // Order passkey has been created. Send confirmation text and email
                {

                    //Get the email template file
                    string path = string.Concat(_hostingEnvironment.ContentRootPath, "//templates//OrderConfirmationEmail.html");
                    var text = System.IO.File.ReadAllText(path);

                    // Escape curly braces and change triangle unicode braces to curly braces for string.format injection
                    var switchOutDoubleQuotes = text.Replace("\"", "'");
                    var escapedLeftBrace = switchOutDoubleQuotes.Replace("{", "{{");
                    var escapedRightBrace = escapedLeftBrace.Replace("}", "}}");
                    var addedLeftFormatPlaceholder = escapedRightBrace.Replace("&#9001;", "{");
                    var addedRightFormatPlaceholder = addedLeftFormatPlaceholder.Replace("&#9002;", "}");
                    var email = String.Format(addedRightFormatPlaceholder, vm.FriendlySubmitted, unhashed, vm.TopicName, vm.Price, serviceFee, vm.Charge, orderId.ToString());
                    
                    //Send the email
                    var mailTemplate = new PasskeyEmail() { Email = vm.UserEmail, Subject = "Stutor order confirmation passkey" };
                    mailTemplate.Body = email;
                    _emailService.SendPasskeyEmail(mailTemplate);

                    //Send the confirmation text message to the user
                    var smsService = new SmsService(_smsSettings);
                    var userPhone = _db.User.FirstOrDefault(u => u.Id == vm.UserId).Phone;
                    smsService.SendConfirmation(orderId, unhashed, userPhone);

                    //Send the text message to the expert
                    var expertPhone = _expertService.GetPhoneById(vm.ExpertId);
                    smsService.SendClientNumber(vm.UserPhone, expertPhone, vm.TopicName);
                    return 1;
                }

            }
            return 0;
        }

        [HttpPost]
        public int SubmitPasskeys(SubmitPasskeyVm vm)
        {
            //Check if order has already been validated
            //If not check to see if the passkeys are correct and mark the order as validated.
            var stored = _repo.GetOrderPasskey(vm.OrderId).ClientPasskey;
            var result = _repo.AuthenticatePasskey(vm.OrderId, vm.ClientPasskey, stored);
            return result;
        }

        [HttpPost]
        public JsonResult updateFeedback(Order incomingOrder)
        {
            JsonResult result = Json(new { error = ""});
            try
            {
                var rowsAffected = _repo.UpdateFeedback(incomingOrder);
                result = (rowsAffected > 0) ? Json(new { status = 200, error = "" }) : Json(new { status = 500, error = "" });
            }
            catch (System.Exception ex)
            {
                result = Json(new { status = 500, error = ex.Message });
            }
            return result;
        }

        [HttpPost]
        public IEnumerable<Order> GetAllByUserId([FromBody] string userId)
        {   
            return _repo.GetAllByUserId(userId);
        }
    }
}