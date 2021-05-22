using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories;

namespace stutor_core.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(ApplicationDbContext context)
        {
            _repo = new UserRepository(context);
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
