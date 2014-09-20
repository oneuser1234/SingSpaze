using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SingSpaze.Models.Management;
using System.Web.Security;

namespace SingSpaze.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           return View();
        }

        /// <summary>
        /// backend login
        /// </summary>
        /// <param name="input">class Account</param>
        /// <returns>redirect</returns>
        [HttpPost]
        public ActionResult login(Login input)
        {
            if (input.user_login == "admin" && input.user_password == "admin")
            {
                FormsAuthentication.SetAuthCookie(input.user_login, false);
                return RedirectToAction("Index", "Song");
            }
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

        public ActionResult Test()
        {
            ViewBag.Message = "Test page.";

            return View();
        }
    }
}
