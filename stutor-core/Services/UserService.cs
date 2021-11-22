using stutor_core.Models.Sql;
using stutor_core.Repositories.Interfaces;
using stutor_core.Services.Interfaces;

namespace stutor_core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User Get(string id)
        {
            return _repo.Get(id);
        }

        public bool UpdateStripeCustomerId(string userId, string customerId)
        {
            return _repo.UpdateStripeCustomerId(userId, customerId);
        }

        public bool UpdatePhoneNumber(string userId, string oldPhoneNumber, string newPhoneNumber)
        {
            return _repo.UpdatePhoneNumber(userId, oldPhoneNumber, newPhoneNumber);
        }
    }
}
