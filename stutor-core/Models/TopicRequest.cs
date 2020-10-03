using System.ComponentModel.DataAnnotations;

namespace stutor_core.Models
{
    public class TopicRequest
    {
        public int Id { get; set; }
        
        public int CategoryId { get; set; }

        [MaxLength(75)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
