using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class PopularCategory
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        #region Foreign key mappings

        public virtual Category Category { get; set; }

        #endregion
    }
}
