using System;
using System.ComponentModel.DataAnnotations;
using MoneyMarket.Common;

namespace MoneyMarket.DataAccess.Models
{
    public class TeamNotification : EntityBase
    {
        [Required]
        public int TeamId { get; set; } // foreign key 
        public virtual Team Team { get; set; } // navigation 

        [Required]
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// notification key
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// time interval in minutes
        /// </summary>
        [Required]
        public int TimeInterval { get; set; }

        /// <summary>
        /// last execution time of notification.
        /// </summary>
        [Required]
        public DateTime LastExecutedAt { get; set; }
    }
}
