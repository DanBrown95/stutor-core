using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ExpertId { get; set; }
        public int TopicId { get; set; }
        public double Charge { get; set; }
        public string Status { get; set; }
        public string ClientPasskey { get; set; }
        public string ExpertPasskey { get; set; }
        public DateTime Submitted { get; set; }
        public int callLength { get; set; }
    }
}
