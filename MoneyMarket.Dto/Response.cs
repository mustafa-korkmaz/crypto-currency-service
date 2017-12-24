using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    public class Response : DtoBase
    {
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
    }
}
