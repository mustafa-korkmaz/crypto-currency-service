
namespace MoneyMarket.Dto
{
    public class TeamScope : DtoBase
    {
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 
    }
}
