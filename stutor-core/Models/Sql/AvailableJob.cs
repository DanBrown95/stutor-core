using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class AvailableJob
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
