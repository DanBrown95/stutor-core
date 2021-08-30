using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    [Table("TopicExpertSpecialty")]
    public class TopicExpertSpecialty
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(TopicExpert))]
        public int TopicExpertId { get; set; }

        [Required, ForeignKey(nameof(Specialty))]
        public int SpecialtyId { get; set; }


        #region Foreign key mappings

        public virtual TopicExpert TopicExpert { get; set; }

        public virtual Specialty Specialty { get; set; }

        #endregion
    }
}
