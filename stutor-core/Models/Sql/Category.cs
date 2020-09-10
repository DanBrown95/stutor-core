using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class Category
    {
        public Category()
        {
            this.Topics = new HashSet<Topic>(); 
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }


        public virtual ICollection<Topic> Topics { get; set; }
    }
}
