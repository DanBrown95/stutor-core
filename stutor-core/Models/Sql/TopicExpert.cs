using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class TopicExpert
    {
        public int TopicId { get; set; }
        public string ExpertId { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public string Availability { get; set; }


        public virtual Topic Topic { get; set; }
        public virtual Expert Expert { get; set; }
    }
}
