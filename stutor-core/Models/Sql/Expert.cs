using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Expert")]
    public class Expert
    {
        [Key, Required]
        public string Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public string UserEmail { get; set; }

        [Required, ForeignKey(nameof(Timezone))]
        public int TimezoneId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        #region Foreign key mappings

        public virtual User User { get; set; }
        public virtual Timezone Timezone { get; set; }
        public virtual ICollection<TopicExpert> TopicExpert { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        #endregion
    }
}
