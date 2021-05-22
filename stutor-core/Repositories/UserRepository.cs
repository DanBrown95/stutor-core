using stutor_core.Database;
using stutor_core.Models.Sql;
using System.Linq;

namespace stutor_core.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext _context { get; set; }

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Get(string id)
        {
            return _context.User.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool UpdateStripeCustomerId(string userId, string customerId)
        {
            var record = _context.User.Single(x => x.Id == userId);
            record.CustomerId = customerId;
            return (_context.SaveChanges() == 1);
        }

        public bool UpdatePhoneNumber(string userId, string oldPhoneNumber, string newPhoneNumber)
        {
            var record = _context.User.FirstOrDefault(x => x.Id == userId && x.Phone == oldPhoneNumber);
            if(record == null)
            {
                return false;
            }
            record.Phone = newPhoneNumber;
            record.Phone_Verified = false;
            return (_context.SaveChanges() == 1);
        }
    }
}
