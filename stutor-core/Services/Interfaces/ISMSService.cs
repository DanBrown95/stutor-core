using System.Threading.Tasks;

namespace stutor_core.Services.Interfaces
{
    public interface ISMSService
    {

        string SendConfirmation(int orderId, string passkey, string phone);

        string SendClientNumber(string client, string expert, string topic);

        Task<string> SendVerificationAsync(string phone);

        Task<string> VerifyConfirmationPin(string phone, string pin);
    }
}
