
using System.Collections.Generic;

namespace MoneyMarket.Dto
{
    public class Scope : DtoBase
    {
        public string Name { get; set; }

        /// <summary>
        /// teams of scope
        /// </summary>
        public virtual IEnumerable<TeamScope> TeamScopes { get; set; } // 1=>n relation with dbo.TeamScopes
    }
}
