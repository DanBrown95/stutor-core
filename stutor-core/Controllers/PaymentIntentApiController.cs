using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;

namespace stutor_core.Controllers
{
    [Route("create-payment-intent")]
    [ApiController]
    public class PaymentIntentApiController : Controller
    {
        [HttpPost]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.Items),
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

        private int CalculateOrderAmount(Item[] items)
        {
            // Replace this constant with a calculation of the order's amount
            // Calculate the order total on the server to prevent
            // people from directly manipulating the amount on the client
            return 1400;
        }
        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }
    }
}