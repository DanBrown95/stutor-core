using stutor_core.Models.Sql;
using System.Collections.Generic;
using stutor_core.Services.Interfaces;
using stutor_core.Repositories.Interfaces;

namespace stutor_core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public Order Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<Order> GetAllByUserId(string userId)
        {
            return _repo.GetAllByUserId(userId);
        }

        public OrderPasskey GetOrderPasskey(int orderId)
        {
            return _repo.GetOrderPasskey(orderId);
        }

        public IEnumerable<Order> GetExpertOrdersByUserId(string userId)
        {
            return _repo.GetExpertOrdersByUserId(userId);
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

        public int UpdateStatus(string status, int orderId)
        {
            return _repo.UpdateStatus(status, orderId);
        }

        public int SetRequiresCapture(int orderId)
        {
            return _repo.SetRequiresCapture(orderId);
        }
    }
}
