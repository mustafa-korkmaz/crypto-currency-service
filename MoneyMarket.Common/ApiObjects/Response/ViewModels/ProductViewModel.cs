using System;
using System.Collections.Generic;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Model returned by FeaturedController actions.
    /// </summary>
    public class ProductViewModel
    {
        /// <summary>
        /// Product id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product desc
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// product Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// product ImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// product web site id
        /// </summary>
        public int WebSiteId { get; set; }

        /// <summary>
        /// product web site name
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
        /// product's followers count
        /// </summary>
        public int FollowerTotal { get; set; }

        /// <summary>
        /// if current user tracks the product or not
        /// </summary>
        public bool FollowedByCurrentUser { get; set; }

        /// <summary>
        /// company name similar as WebSite.Name
        /// </summary>
        public int Company { get; set; }

        /// <summary>
        /// price or quantity history of product
        /// </summary>
        public IEnumerable<ProductHistoryViewModel> Histories { get; set; }
    }

    public class ProductHistoryViewModel
    {
        /// <summary>
        /// Price of Product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity  of Product
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// ModificationType of ProductHistory
        /// </summary>
        public ModificationType ModificationType { get; set; }

        /// <summary>
        /// create dateTime of ProductHistory
        /// </summary>
        public DateTime CreatedAt { get; set; }

    }
}