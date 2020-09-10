using Microsoft.EntityFrameworkCore;
using stutor_core.Database;
using stutor_core.Models.Sql;
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

        public void Add(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }

        public Order Get(int ID)
        {
            return _context.Order.FirstOrDefault(e => e.Id == ID);
        }

        public IEnumerable<Order> GetAllByUserId(string userId)
        {
            return _context.Order.Where(o => o.UserId == userId).Include(o => o.OrderPasskey).Include(x => x.Topic);
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _context.OrderPasskey.FirstOrDefault(e => e.OrderId == orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _context.Order.Where(o => o.ExpertId == o.Expert.Id && o.Expert.UserId == userId).Include(x => x.Topic);
        }

        public int updateFeedback(Order incomingOrder)
        {
            Order old = _context.Order.First(o => o.Id == incomingOrder.Id);
            old.Rating = incomingOrder.Rating;
            old.AdditionalInfo = incomingOrder.AdditionalInfo;
            return _context.SaveChanges();
        }

    }
}
