using System.ComponentModel.DataAnnotations;

namespace stutor_core.Models.Sql
{
    public class ExpertApplication
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public int TopicId { get; set; }

        [MaxLength(200)]
        public string Availability { get; set; }

        public int? TimezoneId { get; set; }

        [MaxLength(200)]
        public string LinkedinUrl { get; set; }

        [MaxLength(200)]
        public string WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string Certifications { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public int YearsOfExperience { get; set; }
    }
}
