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

        // Get/Refresh
        /// <summary>
        /// refreshes all moneys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("xlm")]
        public IHttpActionResult Refresh()
        {
            SendXlmNotification();
            return Ok();
        }

        private void SendXlmNotification()
        {
           _notificationBusiness.SendXlmNotification();
        }
    }
}