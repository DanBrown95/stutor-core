using stutor_core.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models
{
    public class SubmitIntentVM : Order
    {
        public string TopicName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string FriendlySubmitted { get; set; }
    }
}
