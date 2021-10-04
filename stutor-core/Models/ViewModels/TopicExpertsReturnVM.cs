using System.Collections.Generic;
using stutor_core.Models.Sql;

namespace stutor_core.Models.ViewModels
{
    public class TopicExpertsReturnVM
    {
        public TopicExpertsReturnVM()
        {
            LocalExperts = new List<TopicExpert>();
            DistantExperts = new List<TopicExpert>();
        }

        public IEnumerable<TopicExpert> LocalExperts { get; set; }
        public IEnumerable<TopicExpert> DistantExperts { get; set; }
    }
}
