using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyMarket.Common;

namespace MoneyMarket.DataAccess.Models
{
    /// <summary>
    /// slack teams
    /// </summary>
    public class Team : EntityBase
    {
        /// <summary>
        /// Slack Team Id
        /// </summary>
        [Index(IsUnique = true)]
        [Required]
        [MaxLength(50)]
        public string SlackId { get; set; }

        /// <summary>
        /// Slack Bot Id
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string BotId { get; set; }

        /// <summary>
        /// Slack Team Name
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Slack bot access token
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string BotAccessToken { get; set; }

        /// <summary>
        /// slack team member count
        /// </summary>
        [Required]
        public int MemberCount { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        /// <summary>
        /// is team active?
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresIn { get; set; }

        public virtual ICollection<TeamScope> TeamScopes { get; set; } // 1=>n relation with dbo.events
    }
}
