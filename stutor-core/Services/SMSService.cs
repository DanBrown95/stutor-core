using stutor_core.Configurations;
using stutor_core.Models.Interfaces.SMS;
using stutor_core.Models.SMS;
using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using System;
using Twilio.Rest.Verify.V2.Service;
using System.Linq;
using stutor_core.Services.Interfaces;

namespace stutor_core.Services.Controllers
{
    public class SmsService : ISMSService
    {

        private readonly SMSSettings _smsSettings;

        public SmsService(SMSSettings smsSettings)
        {
            _smsSettings = smsSettings;
        }

        /// <summary>
        /// Send passkey authentication sms to the client
        /// </summary>
        /// <param name="passkey">The client passkey</param>
        /// <param name="orderId">The id of the new order</param>
        /// <param name="phone">The clients phone number</param>
        /// <returns>Status of the sms attempt</returns>
        public string SendConfirmation(int orderId, string passkey, string phone)
        {
            var message = "\'" + passkey + "\' is your temporary Stutor passkey.\n\n" + "Provide this to the expert after your question has been answered.\n\n" +
                "Order#: " + orderId + "\n" +
                "Thank you for using Stutor!";
            var sms = new SMS(phone, message);
            return SendSms(sms).ToString();
        }

        /// <summary>
        /// Send client number to expert
        /// </summary>
        /// <param name="client">Client phone number</param>
        /// <param name="expert">Expert's phone number</param> 
        /// <param name="topic">The topic services are requested for</param>
        /// <returns>Status of the sms attemp</returns>
        public string SendClientNumber(string client, string expert, string topic)
        {
            var message = "Your services are being requested for " + topic + ". Please call " + client + " ASAP. \n\n" +
                "Dont forget to retrieve the passkey from the client after services have been rendered. \n\n" + 
                " Thank you for using Stutor! \n ";
            var sms = new SMS(expert, message);
            return SendSms(sms).ToString();
        }

        /// <summary>
        /// Send an sms using Twilio services
        /// </summary>
        /// <param name="sms">ISMS object</param>
        /// <returns>Status of the sms send attempt</returns>
        private async Task<MessageResource.StatusEnum> SendSms(ISMS sms)
        {
            //var from = anonymous ? _smsSettings.anonymousName : new Twilio.Types.PhoneNumber(_smsSettings.from);
            var from = new Twilio.Types.PhoneNumber(_smsSettings.from);
            MessageResource response;
            try
            {
                response = await MessageResource.CreateAsync(
                    body: sms.Message,
                    from: from,
                    to: new Twilio.Types.PhoneNumber(sms.To)
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return response.Status;
        }

        public async Task<string> SendVerificationAsync(string phone)
        {
            try
            {
                var toPhone = (new string(phone.Take(1).ToArray()) == "+") ? phone : "+1" + phone;

                var verification = await VerificationResource.CreateAsync(
                    to: toPhone,
                    channel: "sms",
                    pathServiceSid: _smsSettings.VerificationServiceSID
                );

                return verification.Status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> VerifyConfirmationPin(string phone, string pin)
        {
            try
            {
                var toPhone = (new string(phone.Take(1).ToArray()) == "+") ? phone : "+1" + phone;

                var verification = await VerificationCheckResource.CreateAsync(
                    to: toPhone,
                    code: pin,
                    pathServiceSid: _smsSettings.VerificationServiceSID
                );
                return verification.Status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}