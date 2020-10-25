using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("TopicExpert")]
    public class TopicExpert
    {
        [Required, ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }

        [Required, ForeignKey(nameof(Expert))]
        public string ExpertId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required, MaxLength(200)]
        public string Availability { get; set; }


        #region Foreign key mappings

        public virtual Topic Topic { get; set; }
        public virtual Expert Expert { get; set; }

        #endregion

    }
}
