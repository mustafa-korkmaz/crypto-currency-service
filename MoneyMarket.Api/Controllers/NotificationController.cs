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
        private readonly NotificationBusiness _notificationBusiness = new NotificationBusiness();

        // Get/xlm
        /// <summary>
        /// refreshes all moneys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("xlm")]
        public IHttpActionResult Xlm()
        {
            SendXlmNotification();
            return Ok();
        }

        private void SendXlmNotification()
        {
           _notificationBusiness.SendXlmNotification();
        }

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