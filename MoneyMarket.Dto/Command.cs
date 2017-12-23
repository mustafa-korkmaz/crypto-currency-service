
namespace MoneyMarket.Dto
{
    public class Command : DtoBase
    {
        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 

        public string Action { get; set; }

        public string ResponseText { get; set; }

    }
}
