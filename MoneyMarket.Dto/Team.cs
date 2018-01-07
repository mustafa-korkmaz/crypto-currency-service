
using System;
using System.Collections.Generic;
using MoneyMarket.Common;

namespace MoneyMarket.Dto
{
    /// <summary>
    /// slack teams
    /// </summary>
    public class Team : DtoBase
    {
        /// <summary>
        /// Slack Team Id
        /// </summary>
        public string SlackId { get; set; }

        /// <summary>
        /// Slack Bot Id
        /// </summary>
        public string BotId { get; set; }

        /// <summary>
        /// Slack channel info to send push notifications
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Slack Team Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Slack bot access token
        /// </summary>
        public string BotAccessToken { get; set; }

        /// <summary>
        /// slack team member count
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// bot language
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// real currency choice (try, usd..)
        /// </summary>
        public MainCurrency MainCurrency { get; set; }

        public Provider Provider { get; set; }

        /// <summary>
        /// team's account type (standart, premium, trial)
        /// default value is Trial
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// is team active?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// team create date (app authorize date)
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// team account type due date
        /// if date.now >= ExpiresIn then change account type to suspended
        /// </summary>
        public DateTime ExpiresIn { get; set; }

        /// <summary>
        /// scope claims of team
        /// </summary>
        public virtual IEnumerable<TeamScope> TeamScopes { get; set; } // 1=>n relation with dbo.TeamScopes

    }
}
