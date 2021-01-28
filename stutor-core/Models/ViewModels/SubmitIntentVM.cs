using stutor_core.Models.Sql;

namespace stutor_core.Models.ViewModels
{
    public class SubmitIntentVM : Order
    {
        public string TopicName { get; set; }
        public string UserPhone { get; set; }
        public string FriendlySubmitted { get; set; }
        public string UserEmail { get; set; }
    }
}
