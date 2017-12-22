using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class Command : EntityBase
    {
        [Required]
        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 

        [MaxLength(20)]
        [Required]
        public string Action { get; set; }

        public string ResponseText { get; set; }

    }
}
