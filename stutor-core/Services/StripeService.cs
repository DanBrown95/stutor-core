using Stripe;
using stutor_core.Services.Interfaces;

namespace stutor_core.Services
{
    public class StripeService : IStripeService
    {
        public bool CustomerHasSources(string customerId)
        {
            if(string.IsNullOrEmpty(customerId))
            {
                return false;
            }
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card",
            };
            var service = new PaymentMethodService();
            StripeList<PaymentMethod> paymentMethods = service.List(
              options
            );
            return paymentMethods.Data.Count > 0;
        }
    }
}
