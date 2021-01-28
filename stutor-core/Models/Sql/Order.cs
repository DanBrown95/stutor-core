using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Order")]
    public class Order
    {
        [Key, Required]
        public int Id { get; set; }

        [ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }

        [ForeignKey(nameof(Expert))]
        public string ExpertId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        /// <summary>
        /// The amount charged to the customers card 
        /// </summary>
        public decimal Charge { get; set; }

        /// <summary>
        /// The price of the expert
        /// </summary>
        public decimal Price { get; set; }

        [MaxLength(15)]
        public string Status { get; set; }

        public DateTime Submitted { get; set; }

        public int CallLength { get; set; }
        public int? Rating { get; set; }
        public string AdditionalInfo { get; set; }

        [MaxLength(100)]
        public string PaymentIntentId { get; set; }


        #region Foreign key mappings

        public virtual OrderPasskey OrderPasskey { get; set; }
        public virtual Expert Expert { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }

        #endregion

    }
}
