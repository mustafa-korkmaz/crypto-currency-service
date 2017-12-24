
namespace MoneyMarket.Dto
{
    public class Command : DtoBase
    {
        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 

        /// <summary>
        /// business method name to process command
        /// </summary>

        public string Action { get; set; }

        /// <summary>
        /// command text posted from slack channel. (eg. 'l set en')
        /// db equivalent will be 'l set @p0'
        /// </summary>
        public string Text { get; set; }

        public string ResponseText { get; set; }
    }
}
