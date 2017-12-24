using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.DataAccess.Models
{
    public class Command : EntityBase
    {
        [Required]
        public int ScopeId { get; set; } // foreign key 
        public virtual Scope Scope { get; set; } // navigation 

        /// <summary>
        /// business method name to process command
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string Action { get; set; }

        /// <summary>
        /// command text posted from slack channel. (eg. 'l set en')
        /// db equivalent will be 'l set @p0'
        /// </summary>
        [MaxLength(255)]
        [Required]
        public string Text { get; set; }

        public string ResponseText { get; set; }

    }
}
