using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by CustomerController actions.
    /// </summary>
    public class CustomerViewModel
    {
        /// <summary>
        /// Customer id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// firm id
        /// </summary>
        public int FirmId { get; set; }

        /// <summary>
        /// firm name
        /// </summary>
        public string FirmName { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Customer status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Customer status text
        /// </summary>
        public string StatusText => Statics.GetStatusText(Status);
    }
}