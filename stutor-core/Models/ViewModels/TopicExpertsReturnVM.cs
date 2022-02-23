using System.Collections.Generic;
using stutor_core.Models.Sql.customTypes;

namespace stutor_core.Models.ViewModels
{
    public class TopicExpertsReturnVM
    {
        public TopicExpertsReturnVM()
        {
            LocalExperts = new List<TopicExpertsByTopic>();
            DistantExperts = new List<TopicExpertsByTopic>();
        }

        public IEnumerable<TopicExpertsByTopic> LocalExperts { get; set; }
        public IEnumerable<TopicExpertsByTopic> DistantExperts { get; set; }
    }
}
