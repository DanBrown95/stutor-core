using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Topic")]
    public class Topic
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required, MaxLength(75)]
        public string Name { get; set; }


        #region Foreign key mappings

        public virtual Category Category { get; set; }
        public virtual TopicExpert TopicExpert { get; set; }
        public virtual ICollection<Specialty> Specialty { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<ExpertApplication> ExpertApplication { get; set; }

        #endregion

    }
}
