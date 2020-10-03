using System.Collections.Generic;
using stutor_core.Models.Sql;

namespace stutor_core.Models
{
    public class TopicExpertsReturnVM
    {
        public IEnumerable<TopicExpert> LocalExperts { get; set; }
        public IEnumerable<TopicExpert> DistantExperts { get; set; }
    }
}
