using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Timezone")]
    [Obsolete]
    public class Timezone
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(75)]
        public string FriendlyName { get; set; }

        [Required, MaxLength(75)]
        public string TZName { get; set; }


        #region Foreign key mappings

        public virtual Expert Expert { get; set; }
        public virtual ExpertApplication ExpertApplication { get; set; }

        #endregion

    }
}
