using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class TeamScope : EntityBase
    {
        [Required]
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        [Required]
        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 
    }
}
