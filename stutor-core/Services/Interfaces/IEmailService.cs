using stutor_core.Models;
using System.Threading.Tasks;

namespace stutor_core.Services.Interfaces
{
    public interface IEmailService
    {
        void SendContactUsEmail(ContactForm details);
        void SendPasskeyEmail(PasskeyEmail emailSettings);
    }
}
