using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using stutor_core.Database;
using stutor_core.Services;

namespace stutor_core.Controllers
{
    [Route("create-payment-intent")]
    [Authorize]
    [ApiController]
    public class PaymentIntentApiController : Controller
    {
        ApplicationDbContext _db;
        ExpertService _expertService;
        private readonly decimal serviceFee = 2.50M;

        public PaymentIntentApiController(ApplicationDbContext db)
        {
            _db = db;
            _expertService = new ExpertService(_db);
        }

        [HttpPost]
        public ActionResult Create([FromBody] PaymentIntentCreateRequest purchase)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(purchase.ExpertId, purchase.TopicId),
                Currency = "usd",
            });
            return Json(new { clientSecret = paymentIntent.ClientSecret });

            // Alternatively, set up a webhook to listen for the payment_intent.succeeded event
            // and attach the PaymentMethod to a new Customer
            // var customers = new CustomerService();
            // var customer = customers.Create(new CustomerCreateOptions());
            // var paymentIntents = new PaymentIntentService();
            // var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            // {
            //   Customer = customer.Id,
            //   SetupFutureUsage = "off_session",
            //   Amount = CalculateOrderAmount(request.Items),
            //   Currency = "usd",
            // });
            // return Json(new { clientSecret = paymentIntent.ClientSecret });
            //return Json("true");
        }

        public void ChargeCustomer(string customerId)
        {
            // Lookup the payment methods available for the customer
            var paymentMethods = new PaymentMethodService();
            var availableMethods = paymentMethods.List(new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card",
            });
            // Charge the customer and payment method immediately
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = 1099,
                Currency = "usd",
                Customer = customerId,
                PaymentMethod = availableMethods.Data[0].Id,
                OffSession = true,
                Confirm = true
            });
            if (paymentIntent.Status == "succeeded")
                Console.WriteLine("✅ Successfully charged card off session");
        }

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
}