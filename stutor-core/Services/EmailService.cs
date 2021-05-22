using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using stutor_core.Models;
using stutor_core.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace stutor_core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public void SendContactUsEmail(ContactForm details)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _configuration["Email:SupportEmail"],
                    Password = _configuration["Email:SupportPassword"]
                };

                client.UseDefaultCredentials = false;
                client.Credentials = credential;
                client.Host = _configuration["Email:Host"];
                client.Port = int.Parse(_configuration["Email:Port"]);
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(_configuration["Email:SupportEmail"]));
                    emailMessage.From = new MailAddress(details.Email);
                    emailMessage.ReplyToList.Add(new MailAddress(details.Email));
                    emailMessage.Subject = details.Subject;
                    emailMessage.Body = details.FormattedMessage;
                    client.Send(emailMessage);
                }
            }
        }

        public void SendPasskeyEmail(PasskeyEmail vm)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        //UserName = _configuration["Email:NoreplyEmail"],
                        //Password = _configuration["Email:NoreplyPassword"]
                        UserName = _configuration["Email:SupportEmail"],
                        Password = _configuration["Email:SupportPassword"]
                    };

                    // Set up the email settings
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Host = _configuration["Email:Host"];
                    client.Port = int.Parse(_configuration["Email:Port"]);
                    client.EnableSsl = true;

                    using (var emailMessage = new MailMessage())
                    {
                        emailMessage.To.Add(new MailAddress(vm.Email));
                        emailMessage.From = new MailAddress(_configuration["Email:SupportEmail"]);
                        emailMessage.Subject = vm.Subject;
                        emailMessage.IsBodyHtml = true;

                        // Attach the email images and set their CID's so they may be referenced in the email body
                        string path = string.Concat(_hostingEnvironment.ContentRootPath, "//templates//images//");
                        emailMessage.Attachments.Add(new Attachment(path + "logo_text_red_transparent.png") { ContentId = "stutorLogo" });
                        //emailMessage.Attachments.Add(new Attachment(path + "okok.gif") { ContentId = "okGif" }); // no longer needed with new email template

                        emailMessage.Headers.Add("Content-Type", "content=text/html; charset=\"UTF-8\"");
                        emailMessage.Body = vm.Body;
                        client.Send(emailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
