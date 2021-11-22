using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Order Get(int ID);

        IEnumerable<Order> GetAllByUserId(string userId);

        OrderPasskey GetOrderPasskey(int orderId);

        IEnumerable<Order> GetExpertOrdersByUserId(string userId);

        int UpdateFeedback(Order incomingOrder);

        int Create(Order order);

        int UpdateStatus(string status, int orderId);

        int CreateOrderPasskey(OrderPasskey orderPasskey);

        int AuthenticatePasskey(int orderId, string incomingPasskey, string storedHash);

        int SetRequiresCapture(int orderId);
    }
}
