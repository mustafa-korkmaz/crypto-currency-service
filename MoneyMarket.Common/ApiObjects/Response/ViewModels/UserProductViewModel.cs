using System;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by ProductController actions.
    /// </summary>
    public class UserProductViewModel
    {
        /// <summary>
        /// UserProduct id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Product desc
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// ImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// WebSite id
        /// </summary>
        public int WebSiteId { get; set; }

        /// <summary>
        /// web site name
        /// </summary>
        public string WebSiteName { get; set; }

        /// <summary>
        /// price of Product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// currency text of Product
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Quantity  of Product
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Total count of Product followers
        /// </summary>
        public int FollowerTotal { get; set; }

        /// <summary>
        /// created dateTime of userProduct 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// created dateTime string of userProduct 
        /// </summary>
        public string CreatedAtText => CreatedAt.ToDropDateFormat();
    }

    public class UserProductHistoryViewModel
    {
        /// <summary>
        /// ProductHistory id
        /// </summary>
        public int UserProductHistoryId { get; set; }

        /// <summary>
        /// userProduct id
        /// </summary>
        public int UserProductId { get; set; }

        /// <summary>
        /// Price of Product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity  of Product
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// userProduct status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// userProduct status text
        /// </summary>
        public string StatusText => Statics.GetStatusText(Status);

        /// <summary>
        /// ModificationType of userProductHistory
        /// </summary>
        public ModificationType ModificationType { get; set; }

        /// <summary>
        /// create dateTime of userProductHistory
        /// </summary>
        public DateTime CreatedAt { get; set; }

    }
}