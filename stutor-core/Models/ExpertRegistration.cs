using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models
{
    public class ExpertRegistration
    {
        public int selectedTopicId { get; set; }
        public string[] selectedDays { get; set; }
        public string weekdayHours { get; set; }
        public string weekendHours { get; set; }
        public string websiteUrl { get; set; }
        public string linkedinUrl { get; set; }
    }
}
