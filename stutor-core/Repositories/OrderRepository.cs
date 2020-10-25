using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace stutor_core.Repositories
{
    public class OrderRepository
    {
        private ApplicationDbContext _context { get; set; }

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Get(int ID)
        {
            return _context.Order.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Order> GetAllByUserEmail(string userEmail)
        {
            var result = from o in _context.Order

                         where o.UserEmail == userEmail

                         select new Order() { Id = o.Id, Submitted = o.Submitted, Status = o.Status, Charge = o.Charge, Topic = o.Topic, ExpertId = o.ExpertId, Rating = o.Rating, AdditionalInfo = o.AdditionalInfo };
            return result;
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _context.OrderPasskey.FirstOrDefault(e => e.OrderId == orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserEmail(string userEmail)
        {
            var result = from o in _context.Order

                         where o.ExpertId == o.Expert.Id && o.Expert.UserEmail == userEmail

                         select new Order() { Id = o.Id, Submitted = o.Submitted, Status = o.Status, CallLength = o.CallLength, Price = o.Price, Topic = o.Topic };
            
            return result;
        }

        public int UpdateFeedback(Order incomingOrder)
        {
            Order old = _context.Order.First(o => o.Id == incomingOrder.Id);
            old.Rating = incomingOrder.Rating;
            old.AdditionalInfo = incomingOrder.AdditionalInfo;
            return _context.SaveChanges();
        }

        /// <param name="order"></param>
        /// <returns name="Id">The Id of the newly inserted order</returns>
        public int Create(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        /// <param name="orderPasskey"></param>
        /// <returns name="Id">The Id of the newly inserted orderPasskey</returns>
        public int CreateOrderPasskey(OrderPasskey orderPasskey)
        {
            _context.OrderPasskey.Add(orderPasskey);
            _context.SaveChanges();
            return orderPasskey.Id;
        }

        public int AuthenticatePasskey(int orderId, string incomingPasskey, string storedHash)
        {
            if (PasskeySecurity.Authenticate(incomingPasskey, storedHash))
            {
                var order = _context.Order.First(o => o.Id == orderId);
                if(order == null)
                {
                    return 0;
                }
                order.Status = "Completed";
                return _context.SaveChanges();
            }
            return 0;
        }

    }
}
