using System.Collections.Generic;

namespace stutor_core.Models.Sql.customTypes
{
    public class TopicExpertsByTopic
    {
        public int Id { get; set; }
        public string Availability { get; set; }
        public string ExpertId { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CustomerId { get; set; }

        public virtual ICollection<TopicExpertSpecialty> TopicExpertSpecialty { get; set; }
    }
}
