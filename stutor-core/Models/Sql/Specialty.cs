using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    [Table("Specialty")]
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }
        public string Name { get; set; }

        #region Foreign key mappings

        public virtual ICollection<TopicExpertSpecialty> TopicExpertSpecialty { get; set; }

        public virtual Topic Topic { get; set; }

        #endregion
    }
}
