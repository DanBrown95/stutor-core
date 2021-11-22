namespace stutor_core.Services.Interfaces
{
    public interface IStripeService
    {
        bool CustomerHasSources(string customerId);
    }
}
