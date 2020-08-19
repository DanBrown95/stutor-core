using stutor_core.Configurations;
using stutor_core.Models.Interfaces.SMS;
using stutor_core.Models.SMS;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace stutor_core.Services.Controllers
{
    public class SMSService : Controller
    {

        private readonly SMSSettings _smsSettings;

        public SMSService(SMSSettings smsSettings)
        {
            _smsSettings = smsSettings;
        }

        /// <summary>
        /// Send passkey authentication sms to the client
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Status of the sms attempt</returns>
        [HttpPost]
        public string SendAuthentication(string orderId)
        {
            /*
             * 
             *  Retrieve the client phone number, and clientPasskeyHash from the DB using the orderId
             *  Unhash the client passkey to embed in the sms message
             *  
             */
            var passkey = "PotatoChip"; // dummy string
            var phone = "3195405044"; // dummy phone


            var message = "Your client passkey is" + passkey + ". Provide this to the expert when he calls after answering your questions. \n\n" +
                " Thank you for using Stutor! \n ";
            var sms = new SMS(phone, message);
            return Send(sms);
        }

        /// <summary>
        /// Send client number to expert
        /// </summary>
        /// <param name="client">Client phone number</param>
        /// <param name="expert">Expert's phone number</param> 
        /// <param name="topic">The topic services are requested for</param>
        /// <returns>Status of the sms attemp</returns>
        [HttpPost]
        public string SendClientNumber(string client, string expert, string topic)
        {
            var message = "Your services are being requested for " + topic + ". Please call " + client + " ASAP. \n\n" +
                "Dont forget to retrieve the passkey from the client after services have been rendered. \n\n" + 
                " Thank you for using Stutor! \n ";
            var sms = new SMS(expert, message);
            return Send(sms);
        }

        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="anonymous">Whether the text should include the sender phone number or send as Down Radar</param>
        /// <param name="sms">ISMS object to send</param>
        /// <returns>Status of the attempt</returns>
        private string Send(bool anonymous, ISMS sms)
        {
            var status = SendSms(anonymous, sms);
            return status.ToString();
        }

        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="sms">ISMS object to send</param>
        /// <returns>Status of the sms attemp</returns>
        private string Send(ISMS sms)
        {
            return Send(false, sms);
        }

        /// <summary>
        /// Send an sms using Twilio services
        /// </summary>
        /// <param name="anonymous">Show the twilio phone number to the recipient or send as an alphanumeric senderID </param>
        /// <param name="sms">ISMS object</param>
        /// <returns>Status of the sms send attempt</returns>
        private MessageResource.StatusEnum SendSms(bool anonymous, ISMS sms)
        {
            string accountSid = _smsSettings.accountSid;
            string authToken = _smsSettings.authToken;

            TwilioClient.Init(accountSid, authToken);
            var from = anonymous ? _smsSettings.anonymousName : new Twilio.Types.PhoneNumber(_smsSettings.from);

            var response = MessageResource.Create(
                body: sms.Message,
                from: from,
                to: new Twilio.Types.PhoneNumber(sms.To)
            );
            return response.Status;
        }
    }
}