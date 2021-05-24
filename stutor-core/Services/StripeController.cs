using Stripe;

namespace stutor_core.Services
{
    public class StripeController
    {
        public bool CustomerHasSources(string customerId)
        {
            if(customerId == null)
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
