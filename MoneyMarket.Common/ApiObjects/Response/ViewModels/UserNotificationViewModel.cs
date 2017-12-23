using System;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Common.ApiObjects.Response.ViewModels
{
    /// <summary>
    ///  Api Models returned by Notification actions.
    /// </summary>
    public class UserNotificationViewModel
    {
        /// <summary>
        /// Notification id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Notification text content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// notification payload key object's id property
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// NotificationType enum value
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// NotificationType text value
        /// </summary>
        public string NotificationTypeText => Statics.GetNotificationTypeText(NotificationType);

        /// <summary>
        /// NotificationStatus enum value
        /// </summary>
        public NotificationStatus NotificationStatus { get; set; }

        /// <summary>
        /// NotificationStatus text value
        /// </summary>
        public string NotificationStatusText => Statics.GetNotificationStatusText(NotificationStatus);

        /// <summary>
        /// created dateTime of Notification 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// created dateTime string Notification 
        /// </summary>
        public string CreatedAtText => CreatedAt.ToDropDateFormat();

        /// <summary>
        /// notification owner
        /// </summary>
        public string Owner { get; set; }

        public NotificationProduct Product { get; set; }
    }

    public class NotificationProduct
    {
        /// <summary>
        /// product id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ProductImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }
    }
}