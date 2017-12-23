using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by AlarmController actions.
    /// </summary>
    public class AlarmViewModel
    {
        /// <summary>
        /// Setting name
        /// </summary>
        public int UserProductId { get; set; }
        /// <summary>
        /// Setting name
        /// </summary>
        public int SettingId { get; set; }

        /// <summary>
        /// ExactValue for Notify on Exact Price setting
        /// </summary>
        public decimal? ExactValue { get; set; }
    }
}