using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class Expert
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int TimezoneId { get; set; }


        public virtual Timezone Timezone { get; set; }
        public virtual TopicExpert TopicExpert { get; set; }
    }
}
