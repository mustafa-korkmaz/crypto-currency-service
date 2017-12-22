using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class Setting : EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        [Required]
        [MaxLength(50)]
        public string Value { get; set; }
    }
}
