using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingSpaze.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        
        public ActionResult Index()
        {
            string apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Song", action = "List" });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            return View();
        }

    }
}
