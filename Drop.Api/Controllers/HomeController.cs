using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using MoneyMarket.Business.Common;
using MoneyMarket.Business.HttpClient;
using MoneyMarket.Business.Slack;
using MoneyMarket.Common;
using MoneyMarket.Common.ApiObjects.Request;
using MoneyMarket.Common.Helper;
using MoneyMarket.Common.ApiObjects.Request.SlackApp;
using MoneyMarket.Common.ApiObjects.Response;
using MoneyMarket.Common.ApiObjects.Response.SlackApp;
using MoneyMarket.Dto;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grant()
        {
            AppSettingsReader configurationAppSettings = new AppSettingsReader();

            var clientId = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientId, typeof(string));

            var scope = (string)configurationAppSettings.GetValue(ConfigKeys.SlackScope, typeof(string));

            var grantUrl = string.Format(ApiUrl.SlackGrant, clientId, scope, ApiUrl.SlackRedirectUri);

            return new RedirectResult(grantUrl);
        }

        public ActionResult PrivacyPolicy()
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
            AppSettingsReader configurationAppSettings = new AppSettingsReader();

            var clientId = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientId, typeof(string));

            var clientSecret = (string)configurationAppSettings.GetValue(ConfigKeys.SlackClientSecret, typeof(string));

            var request = new OAuthRequest
            {
                client_secret = clientSecret,
                client_id = clientId,
                code = code,
                redirect_uri = ApiUrl.SlackRedirectUri
            };

            // login via web api 
            var apiInvoker = SlackApiClient.Instance;
            var resp = await apiInvoker.InvokeApi<OAuthRequest, OAuthResponse>(ApiUrl.SlackOAuth, request);

            var isGranted = resp.ResponseData.ok;

            if (isGranted)
            {
                SaveSlackTeam(resp.ResponseData);
            }

            ViewBag.Error = resp.ResponseData.error ?? "";
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
                CreatedAt = DateTime.UtcNow
            };


            var teamBusiness = new TeamBusiness();

            teamBusiness.Add(team);
        }

    }
}
