using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SingSpaze.Models;

namespace SingSpaze.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Register()
        {
            string apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Account", action = "Register" });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            return View();
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            string apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Account", action = "Login" });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
            //string apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Account", action = "Logout" });
            //ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            //return View();
        }

        public ActionResult MyProfile()
        {
            int id = 1;
            string apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Account", action = "Profile",id = id });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            apiUri = Url.HttpRouteUrl("SingSpazeApi", new { controller = "Account", action = "Edit", id = id });
            ViewBag.ApiUrl2 = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            return View();
        }
    }
}
