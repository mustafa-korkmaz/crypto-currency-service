using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by SportsFieldController actions.
    /// </summary>
    public class SportsFieldViewModel
    {
        /// <summary>
        /// SportsField id
        /// </summary>
        public int SportsFieldId { get; set; }

        /// <summary>
        /// SportsField name
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
        /// SportsField status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// SportsField status text
        /// </summary>
        public string StatusText => Statics.GetStatusText(Status);
    }
}