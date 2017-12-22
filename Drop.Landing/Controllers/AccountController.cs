using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drop.Landing.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index(string name)
        {
            HttpCookie cookie = new HttpCookie("access_token", "4Gjnl-8Elup2VQmNa25UwfHENW2_vK6WOxQU1h9cnAoLLhLPsG_NnwoDtGJir8kbDqbPjdK_LGtAS_4lij0D03avdfS9u5T30cR0NND6g_DBxudnf6HNXcUsMe05lW3Q-LANCpW5s0V1tCwAy0jii_IsgwgweVpfYTfq9aDCwQ07YQT5m37H0Wzen3N-d9TDeD7cm0FLQacXT9I3hjGXkdgaIWH2t3hQWqEfPs3rMgukC7Hox2iB_zduyz_lYSCqVLSOgWtjKJEH2XTnCM_z9tG-4acMX0letnRDY8Wvna3RaxDgpftcxzPHRn4Cw_Piqz-mXGJhxoHy2y8wIUvANz5yBWK1JM6W3kcb1GXO8rtxvMCK3gDR8UKXe6IZXuIylBsydGLQL_B8RJfNbtZHdMzyEl6Jfgk9AeADMjcW1BXw4GzpHbY1DDr6kiE3mP2i3wAPe_Q6R2goK48RbxBX-2DRaccyq5yUFUw8AjyABwHmSwmAq_TE_rC_tMLd-ZU6TyXpowS_NnpcERzq-5wmPOtkU6Lz2d93__eLzCXxMAM");
            if(!string.IsNullOrEmpty(name))
            cookie.Domain = "raqun.co";
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index","Home");
        }
    }
}