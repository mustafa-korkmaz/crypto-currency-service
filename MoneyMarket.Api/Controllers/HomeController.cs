using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using MoneyMarket.Business.Common;
using MoneyMarket.Business.Slack;
using MoneyMarket.Business.Slack.Integration;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;
using MoneyMarket.Dto;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : Controller
    {
        private readonly TeamBusiness _teamBusiness = new TeamBusiness();

        /// <summary>
        /// home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Grant()
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();

            var clientId = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientId, typeof(string));

            var scope = (string)configurationAppSettings.GetValue(ConfigKeys.SlackScope, typeof(string));

            var grantUrl = string.Format(ApiUrl.SlackGrant, clientId, scope, ApiUrl.SlackRedirectUri);

            return new RedirectResult(grantUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Support()
        {
            return View();
        }

        /// <summary>
        /// Slack integration redirect url
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ActionResult> GrantSuccess([FromUri]string code)
        {
            var slackIntegrationBusiness = new SlackIntegrationBusiness();

            var oAuthResp = await slackIntegrationBusiness.OAuthAccess(code);

            var isGranted = oAuthResp.ok;

            if (isGranted)
            {
                var isSlackTeamExists = IsSlackTeamExists(oAuthResp.team_id);

                //if team already exists, do nothing.
                if (!isSlackTeamExists)
                {
                    SaveSlackTeam(oAuthResp);
                }

                //say hello to new team!
                var resp = await slackIntegrationBusiness.PostMessage(GetWelcomeMessage(oAuthResp.bot.bot_access_token));
                ViewBag.SlackMessage = resp.error ?? resp.message.text;
            }

            ViewBag.Error = oAuthResp.error ?? "";

            return View(isGranted);
        }

        /// <summary>
        /// saves new slack team after successfully granted
        /// </summary>
        private void SaveSlackTeam(OAuthResponse oAuthResp)
        {
            var team = new Team
            {
                Name = oAuthResp.team_name,
                SlackId = oAuthResp.team_id,
                IsActive = true,
                AccountType = AccountType.Trial,
                BotId = oAuthResp.bot.bot_user_id,
                BotAccessToken = oAuthResp.bot.bot_access_token,
                MemberCount = 1,
                Language = Language.Turkish,
                ExpiresIn = CommonBusiness.GetSlackTeamExpirationDate(AccountType.Trial),
                CreatedAt = DateTime.UtcNow,
                TeamScopes = GetTeamScopes(),
                MainCurrency = MainCurrency.Try,
                Provider = Provider.CoinMarketCap
            };

            _teamBusiness.Add(team);
        }

        private ICollection<Dto.TeamScope> GetTeamScopes()
        {
            var scopes = CommonBusiness.GetSlackScopes();

            var teamScopes = new List<Dto.TeamScope>();

            foreach (var scope in scopes)
            {
                teamScopes.Add(new TeamScope
                {
                    ScopeId = scope.Id
                });
            }

            return teamScopes;
        }

        /// <summary>
        /// returns null or a valid slack team.
        /// </summary>
        /// <param name="slackTeamId"></param>
        /// <returns></returns>
        private Dto.Team GetTeamBySlackId(string slackTeamId)
        {
            return _teamBusiness.GetTeamBySlackId(slackTeamId);
        }

        private bool IsSlackTeamExists(string slackTeamId)
        {
            return GetTeamBySlackId(slackTeamId) != null;
        }

        private SlackMessage GetWelcomeMessage(string token)
        {
            return new SlackMessage
            {
                channel = "#general",
                token = token,
                text = SlackBotMessage.Welcome
            };
        }
    }
}
