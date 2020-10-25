using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("User")]
    public class User
    {

        [Key, Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(12)]
        public string Phone { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Firstname { get; set; }

        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public DateTime? Created { get; set; }

        public DateTime? BanStart { get; set; }

        public DateTime? BanEnd { get; set; }

        #region Foreign key mappings

        public virtual Expert Expert { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        #endregion

    }
}
