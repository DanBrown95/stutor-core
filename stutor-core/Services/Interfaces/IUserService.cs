using stutor_core.Models.Sql;

namespace stutor_core.Services.Interfaces
{
    public interface IUserService
    {
        User Get(string id);

        bool UpdateStripeCustomerId(string userId, string customerId);

        bool UpdatePhoneNumber(string userId, string oldPhoneNumber, string newPhoneNumber);
    }
}
