using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("Role")]
    public class Role
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(15)]
        public string Name { get; set; }
    }
}
