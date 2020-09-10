using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Repositories;
using System.Collections.Generic;

namespace stutor_core.Services
{
    public class OrderService
    {
        private readonly OrderRepository _repo;

        public OrderService(ApplicationDbContext context)
        {
            _repo = new OrderRepository(context);
        }

        public Order Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<Order> GetAllByUserId(string userId)
        {
            return _repo.GetAllByUserId(userId);
        }

        public void Add(Order order)
        {
            _repo.Add(order);
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _repo.GetOrderPasskey(orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _repo.GetExpertOrdersByUserId(userId);
        }

        public int updateFeedback(Order incomingOrder)
        {
            return _repo.updateFeedback(incomingOrder);
        }
    }
}
