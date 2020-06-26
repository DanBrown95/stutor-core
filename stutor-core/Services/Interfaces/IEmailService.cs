using stutor_core.Models;
using System.Threading.Tasks;

namespace stutor_core.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(ContactForm details);
    }
}
