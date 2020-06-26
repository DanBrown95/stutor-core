using Microsoft.Extensions.Configuration;
using stutor_core.Models;
using stutor_core.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace stutor_core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(ContactForm details)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _configuration["Email:Email"],
                    Password = _configuration["Email:Password"]
                };

                client.Credentials = credential;
                client.Host = _configuration["Email:Host"];
                client.Port = int.Parse(_configuration["Email:Port"]);
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(_configuration["Email:Email"]));
                    emailMessage.From = new MailAddress(details.Email);
                    emailMessage.ReplyToList.Add(new MailAddress(details.Email));
                    emailMessage.Subject = details.Subject;
                    emailMessage.Body = details.FormattedMessage;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }
    }
}
