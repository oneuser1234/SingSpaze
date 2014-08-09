using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SingSpaze.Models;
using SingSpaze.Models.Management;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.EntityClient;
using System.Globalization;

namespace SingSpaze.Controllers
{
    [Authorize]
    public class SongController : Controller
    {

        private singspazeEntities db = new singspazeEntities();

        //
        // GET: /Song/

        public ActionResult Index()
        {
            List<songdata> song = (from x in db.song.ToList()
                                   join a in db.album.ToList() on x.song_albumId equals a.album_id
                                   join ar in db.artist.ToList() on x.song_artistId equals ar.artist_id
                                   join c in db.contentpartner.ToList() on x.song_contentPartnerId equals c.contentpartner_id
                                   join l in db.language.ToList() on x.song_languageId equals l.language_id
                                   join g in db.genre.ToList() on x.song_genre equals g.genre_id
                                   join r in db.recordlabel.ToList() on x.song_recordLabelId equals r.recordlabel_id
                                   select new songdata
                                   {
                                       id = x.song_id,
                                       albumdata = new albumdata { id = x.song_albumId, description_TH = a.album_description_th },
                                       artistdata = new artistdata { id = x.song_artistId, description_TH = ar.artist_description_th },
                                       originName = x.song_originName,
                                       engName = x.song_engName,
                                       contentpartnerdata = new contentpartnerdata { id = x.song_contentPartnerId, description = c.contentpartner_description },
                                       languagedata = new languagedata { id = x.song_languageId, description = l.language_description },
                                       filePath = null,
                                       keywords = null,
                                       genredata = new genredata { id = x.song_genre, description = g.genre_description },
                                       length = x.song_length,
                                       lyrics = null,
                                       picture = null,
                                       price = 0,
                                       recordlabeldata = new recordlabeldata { id = x.song_recordLabelId, description = r.recordlabel_description },
                                       releasedDate = x.song_releasedDate,
                                       thumbnail = null,
                                       view = x.song_view,
                                       status = x.song_status
                                   }).ToList();

            return View(song);
        }

        
        //
        // GET: /Song/Create

        public ActionResult Create()
        {

            editsong editsong = new editsong();
            List<SelectListItem> ListGenre = (from x in db.genre.ToList()
                                              select new SelectListItem
                                              {
                                                  Value = x.genre_id.ToString(),
                                                  Text = x.genre_description
                                              }).ToList();


            editsong.GenreList = new SelectList(ListGenre, "Value", "Text");

            List<SelectListItem> ListLanguage = (from x in db.language.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Value = x.language_id.ToString(),
                                                     Text = x.language_description
                                                 }).ToList();

            editsong.LanguageList = new SelectList(ListLanguage, "Value", "Text");

            List<SelectListItem> ListAlbum = (from x in db.album.ToList()
                                              select new SelectListItem
                                              {
                                                  Value = x.album_id.ToString(),
                                                  Text = x.album_description_th
                                              }).ToList();

            editsong.AlbumList = new SelectList(ListAlbum, "Value", "Text");

            List<SelectListItem> ListArtist = (from x in db.artist.ToList()
                                               select new SelectListItem
                                               {
                                                   Value = x.artist_id.ToString(),
                                                   Text = x.artist_description_th
                                               }).ToList();
            editsong.ArtistList = new SelectList(ListArtist, "Value", "Text");

            List<SelectListItem> ListContentPartner = (from x in db.contentpartner.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = x.contentpartner_id.ToString(),
                                                           Text = x.contentpartner_description
                                                       }).ToList();


            editsong.ContentPartnerList = new SelectList(ListContentPartner, "Value", "Text");

            List<SelectListItem> ListRecordLabel = (from x in db.recordlabel.ToList()
                                                    select new SelectListItem
                                                    {
                                                        Value = x.recordlabel_id.ToString(),
                                                        Text = x.recordlabel_description
                                                    }).ToList();

            editsong.RecordLabelList = new SelectList(ListRecordLabel, "Value", "Text");

            return View(editsong);
        }

        //
        // POST: /Song/Create

        [HttpPost]
        public ActionResult Create(editsong song)
        {
            if (ModelState.IsValid)
            {
                song addsong = new song();
                addsong.song_originName = song.Name_TH;
                addsong.song_engName = song.Name_EN;
                addsong.song_artistId = song.Artist;
                addsong.song_albumId = song.Album;
                addsong.song_releasedDate = DateTime.ParseExact(song.ReleasedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                addsong.song_languageId = song.Language;
                addsong.song_genre = song.Genre;
                addsong.song_recordLabelId = song.RecordLabel;
                addsong.song_length = Convert.ToDecimal(song.Length); //wrong for linq
                addsong.song_lyrics = song.Lyrics;
                addsong.song_status = song.Status;
                addsong.song_addedDate = DateTime.Now;

                db.song.AddObject(addsong);
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

            editsong editsong = new editsong()
            {
                Id = song.song_id,
                Name_TH = song.song_originName,
                Name_EN = song.song_engName,
                Album = song.song_albumId,
                Artist = song.song_artistId,
                Genre = song.song_genre,
                RecordLabel = song.song_recordLabelId,
                Language = song.song_languageId,
                Length = song.song_length,
                Lyrics = song.song_lyrics,
                ReleasedDate = song.song_releasedDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture),
                //ReleasedDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Status = song.song_status

            };
            List<SelectListItem> ListGenre = (from x in db.genre.ToList()
                                              select new SelectListItem
                                              {
                                                  Value = x.genre_id.ToString(),
                                                  Text = x.genre_description
                                              }).ToList();
            
            
            editsong.GenreList = new SelectList(ListGenre, "Value", "Text");

            List<SelectListItem> ListLanguage = (from x in db.language.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Value = x.language_id.ToString(),
                                                     Text = x.language_description
                                                 }).ToList();
            
            editsong.LanguageList = new SelectList(ListLanguage, "Value", "Text");

            List<SelectListItem> ListAlbum = (from x in db.album.ToList()
                                              select new SelectListItem
                                              {
                                                  Value = x.album_id.ToString(),
                                                  Text = x.album_description_th
                                              }).ToList();

            editsong.AlbumList = new SelectList(ListAlbum, "Value", "Text");

            List<SelectListItem> ListArtist = (from x in db.artist.ToList()
                                               select new SelectListItem
                                               {
                                                   Value = x.artist_id.ToString(),
                                                   Text = x.artist_description_th
                                               }).ToList();
            editsong.ArtistList = new SelectList(ListArtist, "Value", "Text");

            List<SelectListItem> ListContentPartner = (from x in db.contentpartner.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Value = x.contentpartner_id.ToString(),
                                                           Text = x.contentpartner_description
                                                       }).ToList();

            
            editsong.ContentPartnerList = new SelectList(ListContentPartner, "Value", "Text");

            List<SelectListItem> ListRecordLabel = (from x in db.recordlabel.ToList()
                                                    select new SelectListItem
                                                    {
                                                        Value = x.recordlabel_id.ToString(),
                                                        Text = x.recordlabel_description
                                                    }).ToList();

            editsong.RecordLabelList = new SelectList(ListRecordLabel, "Value", "Text");

            return View(editsong);
        }

        //
        // POST: /Song/Edit/5

        [HttpPost]
        public ActionResult Edit(editsong song)
        {
            if (ModelState.IsValid)
            {
                //db.song.Attach(song);
                //db.ObjectStateManager.ChangeObjectState(song, EntityState.Modified);
                var updatesong = db.song.Single(s => s.song_id == song.Id);
                updatesong.song_originName = song.Name_TH;
                updatesong.song_engName = song.Name_EN;
                updatesong.song_artistId = song.Artist;
                updatesong.song_albumId = song.Album;
                //updatesong.song_releasedDate = DateTime.ParseExact(song.ReleasedDate, "dd/MM/yyyy", null);
                //updatesong.song_releasedDate = DateTime.ParseExact(song.ReleasedDate + " 00:00:00", "dd/MM/yyyy hh:mm:ss", null); //not work for linq
                //DateTime datedata = DateTime.ParseExact(song.ReleasedDate, "dd/MM/yyyy", null);
                //string stringdata = datedata.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                //var data = DateTime.ParseExact(stringdata, "yyyy-MM-dd", null);
                updatesong.song_releasedDate = DateTime.ParseExact(song.ReleasedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                updatesong.song_languageId = song.Language;
                updatesong.song_genre = song.Genre;
                updatesong.song_recordLabelId = song.RecordLabel;
                updatesong.song_length = song.Length; //wrong
                updatesong.song_lyrics = song.Lyrics;
                updatesong.song_status = song.Status;

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