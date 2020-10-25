using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stutor_core.Models.Sql
{
    [Table("OrderPasskey")]
    public class OrderPasskey
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [Required, MaxLength(250)]
        public string ClientPasskey { get; set; }


        #region Foreign key mappings

        public virtual Order Order { get; set; }

        #endregion

    }
}
