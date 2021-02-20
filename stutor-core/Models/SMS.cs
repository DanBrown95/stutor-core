using stutor_core.Models.Interfaces.SMS;

namespace stutor_core.Models.SMS
{
    public class SMS : ISMS
    {
        #region Constructors

        public SMS(string to, string message)
        {
            To = "+1"+to;
            Message = message;
        }

        #endregion

    }
}