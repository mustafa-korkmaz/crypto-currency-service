using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drop.Landing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[OutputCache(Duration = 1000 * 60 * 15)]
        public ActionResult Index()
        {
            return View();
        }
    }
}