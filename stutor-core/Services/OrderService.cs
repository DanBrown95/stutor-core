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

        public IEnumerable<Order> GetAllByUserEmail(string userEmail)
        {
            return _repo.GetAllByUserEmail(userEmail);
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _repo.GetOrderPasskey(orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserEmail(string userEmail)
        {
            return _repo.GetExpertOrdersByUserEmail(userEmail);
        }

        public int UpdateFeedback(Order incomingOrder)
        {
            return _repo.UpdateFeedback(incomingOrder);
        }

        public int Create(Order order)
        {
            return _repo.Create(order);
        }

        public int CreateOrderPasskey(OrderPasskey orderPasskey)
        {
            return _repo.CreateOrderPasskey(orderPasskey);
        }

        public int AuthenticatePasskey(int orderId, string incomingPasskey, string storedHash)
        {
            return _repo.AuthenticatePasskey(orderId, incomingPasskey, storedHash);
        }
    }
}
