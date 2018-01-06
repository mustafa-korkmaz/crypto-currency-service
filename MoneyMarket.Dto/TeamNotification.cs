using System;
using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    public class TeamNotification : DtoBase
    {
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// notification key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// time interval in minutes
        /// </summary>
        public int TimeInterval { get; set; }

        /// <summary>
        /// last execution time of notification.
        /// </summary>
        public DateTime LastExecutedAt { get; set; }
    }
}
