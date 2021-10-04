using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Enumerations;
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

        public IEnumerable<Order> GetAllByUserId(string userId)
        {
            var result = from o in _context.Order

                         where o.UserId == userId && (o.Status != OrderStatus.CancelationPending && o.Status != OrderStatus.Canceled)

                         select new Order() { Id = o.Id, Submitted = o.Submitted, Status = o.Status, Charge = o.Charge, Topic = o.Topic, ExpertId = o.ExpertId, Rating = o.Rating, AdditionalInfo = o.AdditionalInfo };
            return result;
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _context.OrderPasskey.FirstOrDefault(e => e.OrderId == orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            var result = from o in _context.Order

                         where o.ExpertId == o.Expert.Id && o.Expert.UserId == userId

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
            var expert = _context.TopicExpert.Where(x => x.ExpertId == order.ExpertId && x.TopicId == order.TopicId).First();
            if(order.Price != expert.Price) // ensure the price passed from the view is not tampered with and accurate.
            {
                return 0;
            }
            _context.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public int UpdateStatus(string status, int orderId)
        {
            if (orderId == 0) return -1;
            var order = _context.Order.Where(x => x.Id == orderId).FirstOrDefault();
            if (order.Id > 0)
            {
                order.Status = status;
                var result = _context.SaveChanges();
                return result;
            }
            return 0;
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
                order.Status = OrderStatus.Completed;
                return _context.SaveChanges();
            }
            return 0;
        }

        public int SetRequiresCapture(int orderId)
        {
            if (orderId <= 0) return 0;

            var order = _context.Order.Where(x => x.Id == orderId).First();
            if(order != null)
            {
                order.RequiresCapture = true;
                _context.SaveChanges();
            }
            return 0;
        }

    }
}
