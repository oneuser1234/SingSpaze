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
using SingSpaze.Models.Management;

namespace SingSpaze.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private singspazeEntities db = new singspazeEntities();

        public ActionResult Index()
        {
            List<userdata> user = (from x in db.user.ToList()
                                   select new userdata
                                   {
                                       id = x.user_id,
                                       Firstname = x.user_firstname,
                                       Lastname = x.user_lastname,
                                       username = x.user_login,
                                       Email = x.user_email,
                                       fbUserId = x.user_fbUserId
                                   }).ToList();

            return View(user);
        }

        public ActionResult Create()
        {
            return View(new editaccount());
        }

        [HttpPost]
        public ActionResult Create(editaccount user)
        {
            if (ModelState.IsValid)
            {
                user adduser = new user()
                {
                    user_login = user.username,
                    user_password = user.password,
                    user_firstname = user.firstname,
                    user_lastname = user.lastname,
                    user_fbUserId = user.facebookid,
                    user_email = user.email
                };
                

                db.user.AddObject(adduser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(int id = 0)
        {
            user user = db.user.Single(s => s.user_id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            editaccount edituser = new editaccount()
            {
                id = user.user_id,
                username = user.user_login,
                password = user.user_password,
                firstname = user.user_firstname,
                lastname = user.user_lastname,
                email = user.user_email,
                facebookid = user.user_fbUserId

            };


            return View(edituser);
        }

        [HttpPost]
        public ActionResult Edit(editaccount user)
        {
            if (ModelState.IsValid)
            {
                //db.song.Attach(song);
                //db.ObjectStateManager.ChangeObjectState(song, EntityState.Modified);
                var updateuser = db.user.Single(s => s.user_id == user.id);
                updateuser.user_login = user.username;
                if (user.password != "")
                    updateuser.user_password = user.password;
                updateuser.user_firstname = user.firstname;
                updateuser.user_lastname = user.lastname;
                updateuser.user_email = user.email;
                updateuser.user_fbUserId = user.facebookid;
                    
                //string dates = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //DateTime date = DateTime.ParseExact(dates, "yyyy-MM-dd HH:mm:ss", null);
                //updatesong.song_addedDate = date ;


                db.SaveChanges();

                //using (SqlConnection connection = new SqlConnection("server=localhost;User Id=root;database=singspaze"))
                //{
                //    connection.Open();
                //    SqlCommand command = new SqlCommand();
                //    command.CommandText = "UPDATE song SET song_length = " + song.Length + " and song_releasedDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where song_id ="+song.Id;
                //    command.CommandTimeout = 15;
                //    command.CommandType = CommandType.Text;
                //    connection.Close();

                //}
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int id = 0)
        {
            user user = db.user.Single(s => s.user_id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.user.Single(s => s.user_id == id);
            db.user.DeleteObject(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","home");
        }

    }
}
