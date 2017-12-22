using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class Scope : EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
