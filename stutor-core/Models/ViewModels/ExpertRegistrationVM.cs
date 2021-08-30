using System.ComponentModel.DataAnnotations;

namespace stutor_core.Models.ViewModels
{
    public class ExpertRegistrationVM
    {
        public string UserId { get; set; }

        public int TopicId { get; set; }
        
        public string[] SelectedDays { get; set; }
        
        public string WeekdayHours { get; set; }
        
        public string WeekendHours { get; set; }
        
        [MaxLength(300)]
        public string WebsiteUrl { get; set; }
        
        [MaxLength(300)]
        public string LinkedinUrl { get; set; }
        
        [MaxLength(500)]
        public string Certifications { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
        
        public int YearsOfExperience { get; set; }
        
        public int? TimezoneId { get; set; }

        public string Specialties { get; set; }
    }
}
