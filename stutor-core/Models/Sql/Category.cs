using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            this.Topics = new HashSet<Topic>(); 
        }

        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(74)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }


        #region Foreign key mappings

        public virtual ICollection<Topic> Topics { get; set; }

        #endregion
    }
}
