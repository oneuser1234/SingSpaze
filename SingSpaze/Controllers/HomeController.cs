using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SingSpaze.Models.Management;

namespace SingSpaze.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           return View();
        }

        [HttpPost]
        public ActionResult login(Account input)
        {
            if(input.user_login == "admin" && input.user_password == "admin")
                return RedirectToAction("Index", "Song");
            else
                return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
