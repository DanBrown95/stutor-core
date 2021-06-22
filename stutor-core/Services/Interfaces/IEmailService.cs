using stutor_core.Models;
using System.Net;
using System.Threading.Tasks;

namespace stutor_core.Services.Interfaces
{
    public interface IEmailService
    {
        Task<HttpStatusCode> SendContactUsEmail(ContactForm details);
        void SendPasskeyEmail(PasskeyEmail emailSettings);
    }
}
