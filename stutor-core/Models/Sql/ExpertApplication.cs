using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("ExpertApplication")]
    public class ExpertApplication
    {

        [Key, Required]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required, ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }

        [Required, MaxLength(200)]
        public string Availability { get; set; }

        [Required, ForeignKey(nameof(Timezone))]
        public int? TimezoneId { get; set; }

        [MaxLength(200)]
        public string LinkedinUrl { get; set; }

        [MaxLength(200)]
        public string WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string Certifications { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }


        #region Foreign key mappings

        public virtual User User { get; set; }

        #endregion

    }
}
