using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.ViewModels
{
    public class UpdateTopicExpertSpecialties
    {
        public int TopicExpertId { get; set; }
        public int[] SpecialtyIds { get; set; }
    }
}
