using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class Order
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string ExpertId { get; set; }
        public string UserId { get; set; }
        
        /// <summary>
        /// The amount charged to the customers card 
        /// </summary>
        public decimal Charge { get; set; }

        /// <summary>
        /// The price of the expert
        /// </summary>
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime Submitted { get; set; }
        public int CallLength { get; set; }
        public int? Rating { get; set; }
        public string AdditionalInfo { get; set; }
        public string PaymentIntentId { get; set; }


        public virtual OrderPasskey OrderPasskey { get; set; }
        public virtual Expert Expert { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
