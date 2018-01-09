using System.Threading.Tasks;
using System.Web.Http;
using MoneyMarket.Business.Notification;

namespace MoneyMarket.Api.Controllers
{
    /// <summary>
    /// Notification controller
    /// </summary>
    [RoutePrefix("Notification")]
    public class NotificationController : ApiBaseController
    {
        /// <summary>
        /// sends slack team notifications
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("slack")]
        public IHttpActionResult SendSlackTeamNotifications()
        {
            var teamNotificationBusiness = new TeamNotificationBusiness();
            // async run business, return OK immediately
            Task.Run(() =>
            {
                teamNotificationBusiness.SendSlackTeamNotifications();
            });

            return Ok();
        }

    }
}