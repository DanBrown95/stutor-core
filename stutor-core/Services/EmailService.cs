using Amazon;
using MimeKit;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using stutor_core.Models;
using stutor_core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

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

        public async Task<HttpStatusCode> SendContactUsEmail(ContactForm details)
        {
            var receiver = _configuration["AWSSES:SupportEmail"];

            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = receiver,
                    ReplyToAddresses = new List<string> { details.Email},
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiver }
                    },
                    Message = new Message
                    {
                        Subject = new Content(details.Subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = details.FormattedMessage
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = details.Message
                            }
                        }
                    }
                };
                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                }
                catch (Exception ex)
                {
                    Log.Error("Could not send contactUs email to {receiver}. {message}", receiver, ex.Message);
                    return HttpStatusCode.InternalServerError;
                }
            }
            return HttpStatusCode.OK;
        }

        public void SendPasskeyEmail(PasskeyEmail vm)
        {
            try
            {
                
                // Attach the email images and set their CID's so they may be referenced in the email body
                string path = string.Concat(_hostingEnvironment.ContentRootPath, "//templates//images//");
                var body = new BodyBuilder()
                {
                    HtmlBody = vm.Body
                };
                body.Attachments.Add(path + "logo_text_red_transparent.png").ContentId = "stutorLogo";

                string from = _configuration["AWSSES:NoreplyEmail"];
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("noreply", from));
                message.To.Add(new MailboxAddress(string.Empty, vm.Email));
                message.Subject = vm.Subject;
                message.Body = body.ToMessageBody();

                var stream = new MemoryStream();
                message.WriteTo(stream);

                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast2))
                {
                    var sendRequest = new SendRawEmailRequest
                    {
                        RawMessage = new RawMessage(stream)
                    };
                    try
                    {
                        var response = client.SendRawEmailAsync(sendRequest);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Could not send order confirmation email through SES to {email}. {message}", vm.Email, ex.Message);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error creating order confirmation email. {message}", ex.Message);
                throw ex;
            }
        }
    }
}
