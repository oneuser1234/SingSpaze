using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SingSpaze.Models;

namespace SingSpaze.Controllers
{
    public class SongController : Controller
    {
        private singspazeEntities db = new singspazeEntities();

        //
        // GET: /Song/

        public ActionResult Index()
        {
            return View(db.song.ToList());
        }

        //
        // GET: /Song/Details/5

        public ActionResult Details(int id = 0)
        {
            song song = db.song.Single(s => s.song_id == id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        //
        // GET: /Song/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Song/Create

        [HttpPost]
        public ActionResult Create(song song)
        {
            if (ModelState.IsValid)
            {
                db.song.AddObject(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(song);
        }

        //
        // GET: /Song/Edit/5

        public ActionResult Edit(int id = 0)
        {
            song song = db.song.Single(s => s.song_id == id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        //
        // POST: /Song/Edit/5

        [HttpPost]
        public ActionResult Edit(song song)
        {
            if (ModelState.IsValid)
            {
                db.song.Attach(song);
                db.ObjectStateManager.ChangeObjectState(song, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }

        //
        // GET: /Song/Delete/5

        public ActionResult Delete(int id = 0)
        {
            song song = db.song.Single(s => s.song_id == id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        //
        // POST: /Song/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            song song = db.song.Single(s => s.song_id == id);
            db.song.DeleteObject(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}