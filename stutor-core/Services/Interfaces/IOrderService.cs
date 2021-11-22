using stutor_core.Models.Sql;
using System;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface IOrderService
    {
        Order Get(int id);

        IEnumerable<Order> GetAllByUserId(string userId);

        OrderPasskey GetOrderPasskey(int orderId);

        IEnumerable<Order> GetExpertOrdersByUserId(string userId);

        int UpdateFeedback(Order incomingOrder);

        int Create(Order order);

        int CreateOrderPasskey(OrderPasskey orderPasskey);

        int AuthenticatePasskey(int orderId, string incomingPasskey, string storedHash);

        int UpdateStatus(string status, int orderId);

        int SetRequiresCapture(int orderId);
    }
}
