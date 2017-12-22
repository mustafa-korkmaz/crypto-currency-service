namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Model returned by FeaturedController actions.
    /// </summary>
    [Nest.ElasticsearchType(Name = "ProductViewModel", IdProperty = "Id")]
    public class ProductViewModelEs
    {
        /// <summary>
        /// Product id
        /// </summary>
        public int ProductId { get; set; }

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
        public string Company { get; set; }
        public int WebSiteId { get; set; }
        public int Id { get; set; }
        public string SearchText { get; set; }
    }
}