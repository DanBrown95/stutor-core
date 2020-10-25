using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public string UserEmail { get; set; }

        [Required, ForeignKey(nameof(Role))]
        public int RoleId { get; set; }


        #region Foreign key mappings

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        #endregion

    }
}
