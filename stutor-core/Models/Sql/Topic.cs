using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class Topic
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }


        public virtual Category Category { get; set; }
        public virtual TopicExpert TopicExpert { get; set; }
    }
}
