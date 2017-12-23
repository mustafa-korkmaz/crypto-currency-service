namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by FirmController actions.
    /// </summary>
    public class CryptoCurrencyViewModel
    {
        /// <summary>
        /// CryptoCurrency id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Currency name
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Provider
        /// </summary>
        public string Provider { get; set; }

        public string UsdValue { get; set; }

        public string ModifiedAt { get; set; }
    }
}