using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    public class Response : DtoBase
    {
        public int CommandId { get; set; } // foreign key 
        public virtual Command Command { get; set; } // navigation

        /// <summary>
        /// response text language
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// response text content after command executed successfully
        /// </summary>
        public string SuccessText { get; set; }

        /// <summary>
        /// response text content on command execution error
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// Attemp count for command
        /// </summary>
        public byte Depth { get; set; }
    }
}
