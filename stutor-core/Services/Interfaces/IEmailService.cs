using stutor_core.Models;
using System.Net;
using System.Threading.Tasks;

namespace stutor_core.Services.Interfaces
{
    public interface IEmailService
    {
        Task<HttpStatusCode> SendContactUsEmail(ContactForm details);
        void SendPasskeyEmail(PasskeyEmail emailSettings);
        void SendOrderConfirmationEmail(string customerFirstname, string customerEmail, string passkey, string date, int orderId, decimal price, decimal charge, decimal serviceFee, string topic);
        void SendExpertRequestEmail(string expertEmail, string topic);
    }
}
