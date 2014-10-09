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
using System.IO;
using System.Web.UI;
using System.Text;


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
            List<Song> song = (from x in db.song
                              select new Song
                                 {
                                       Id = x.song_id,
                                       Name = x.song_originName,
                                       Album = db.album.FirstOrDefault(a => a.album_id == x.song_albumId ).album_description_th,
                                       Artist = db.artist.FirstOrDefault(ar => ar.artist_id == x.song_artistId).artist_description_th,
                                       Language = db.language.FirstOrDefault(l => l.language_id == x.song_languageId).language_description,
                                       Genre = db.genre.FirstOrDefault(g => g.genre_id == x.song_genre).genre_description,
                                       Publisher = db.publisherforsong.FirstOrDefault(p => p.publisherforsong_Id == x.publisherforsong_id).publisherforsong_description,
                                       Length = x.song_length,
                                       Status = x.song_status
                                   }).OrderByDescending(x => x.Id).ToList();

            return View(song);
        }

        
        //
        // GET: /Song/Create

        public ActionResult Create()
        {

            Editsong editsong = new Editsong();
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

           // List<SelectListItem> ListContentPartner = (from x in db.contentpartner.ToList()
           //                                            select new SelectListItem
           //                                            {
           //                                               Value = x.contentpartner_id.ToString(),
           //                                                Text = x.contentpartner_description
           //                                            }).ToList();


            //editsong.ContentPartnerList = new SelectList(ListContentPartner, "Value", "Text");

            List<SelectListItem> ListPublisher = (from x in db.publisherforsong.ToList()
                                                    select new SelectListItem
                                                    {
                                                        Value = x.publisherforsong_Id.ToString(),
                                                        Text = x.publisherforsong_description
                                                    }).ToList();

            editsong.PublisherList = new SelectList(ListPublisher, "Value", "Text");

            return View(editsong);
        }

        //
        // POST: /Song/Create

        [HttpPost]
        public ActionResult Create(Editsong song)
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
                addsong.publisherforsong_id = song.Publisher;
                addsong.song_length = song.Length; //wrong for linq
                addsong.song_lyrics = song.Lyrics;
                addsong.song_status = song.Status;
                addsong.song_addedDate = DateTime.Now;

                db.song.Add(addsong);
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

            Editsong editsong = new Editsong()
            {
                Id = song.song_id,
                Name_TH = song.song_originName,
                Name_EN = song.song_engName,
                Album = song.song_albumId,
                Artist = song.song_artistId,
                Genre = song.song_genre,
                Publisher = song.publisherforsong_id,
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

            //List<SelectListItem> ListContentPartner = (from x in db.contentpartner.ToList()
            //                                           select new SelectListItem
            //                                           {
            //                                               Value = x.contentpartner_id.ToString(),
            //                                               Text = x.contentpartner_description
            //                                           }).ToList();

            
            //editsong.ContentPartnerList = new SelectList(ListContentPartner, "Value", "Text");

            List<SelectListItem> ListPublisher = (from x in db.publisherforsong.ToList()
                                                    select new SelectListItem
                                                    {
                                                        Value = x.publisherforsong_Id.ToString(),
                                                        Text = x.publisherforsong_description
                                                    }).ToList();

            editsong.PublisherList = new SelectList(ListPublisher, "Value", "Text");

            return View(editsong);
        }

        //
        // POST: /Song/Edit/5

        [HttpPost]
        public ActionResult Edit(Editsong song)
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
                updatesong.publisherforsong_id = song.Publisher;
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
            db.song.Remove(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
//        [HttpPost]
//        public string import()
//        {
//            if (Request.Files.Count > 0)
//            {
//                var file = Request.Files[0];

//                if (file != null && file.ContentLength > 0)
//                {
//                    var fileName = Path.GetFileName(file.FileName);
//                    var path = Path.Combine(Server.MapPath("~/import/"), fileName);
//                    file.SaveAs(path);

//                    singspazeEntities ldb = new singspazeEntities();
//                    var filepath = Path.Combine(Server.MapPath("~/Picture/song"));
//                    var address = Request.ServerVariables["REMOTE_ADDR"] + @"\Picture\song";
//                    var sql = @"LOAD DATA  INFILE  '" + path.Replace(@"\", @"\\") +@"' IGNORE INTO TABLE  `csv` CHARACTER SET 'tis620' FIELDS TERMINATED BY  ',' ENCLOSED BY  '" + '"' + "' ESCAPED BY  '" + @"\\" + "' LINES TERMINATED BY  '" + @"\r\n" + "' IGNORE 1 LINES;";
//                    ldb.ExecuteStoreCommand("TRUNCATE TABLE  `csv`");
//                    ldb.ExecuteStoreCommand(sql);

//                    ldb.ExecuteStoreCommand(@"UPDATE `csv` SET `Photo`= concat('" + address.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ',''));");
//                    ldb.ExecuteStoreCommand(@"UPDATE `csv` SET `Lyrics`= concat(concat('" + filepath.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ','')),'\\lyrics.txt');");

//                    //Lyrics
//                    string lyrics = "";
//                    string lsql = "";
//                    IEnumerable<string> resultsstring = ldb.ExecuteStoreQuery<string>(@"select Lyrics from csv;");
//                    foreach (string data in resultsstring)
//                    {
//                        try
//                        {
//                            using (StreamReader sr = new StreamReader(data.Replace(@"\", @"\\")))
//                            {
//                                try
//                                {
//                                    lyrics = sr.ReadToEnd();

//                                    lsql = lsql + @"UPDATE `csv` SET `Lyrics`= '" + lyrics + "' where `Lyrics`='" + data.Replace(@"\", @"\\") + "';";
//                                }
//                                catch (Exception e)
//                                {
//                                }
//                            }
//                        }
//                        catch (Exception e)
//                        {
//                            lsql = lsql + @"UPDATE `csv` SET `Lyrics`= '' where `Lyrics`='" + data.Replace(@"\", @"\\") + "';";
//                        }
                        
//                    }
//                    ldb.ExecuteStoreCommand(lsql);
                    
//                    //add artist
//                    ldb.ExecuteStoreCommand(@"ALTER TABLE  `artist`ADD UNIQUE (`artist_description_th`,`artist_description_en`);");
//                    lsql = @"INSERT INTO artist (`artist_description_th`,`artist_description_en`) 
//                             SELECT distinct c.`Artist - TH`, c.`Artist - EN` 
//                             FROM csv as c
//                             ON DUPLICATE KEY UPDATE `artist_description_th`= c.`Artist - TH`,`artist_description_en`= c.`Artist - EN`;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    //add album
//                    ldb.ExecuteStoreCommand(@"ALTER TABLE  `album`ADD UNIQUE (`album_description_th`,`album_description_en`);");
//                    lsql = @"INSERT INTO album (`album_description_th`,`album_description_en`) 
//                             SELECT distinct c.`Album - TH`, c.`Album - EN` 
//                             FROM csv as c
//                             ON DUPLICATE KEY UPDATE `album_description_th`= c.`Album - TH`,`album_description_en`= c.`Album - EN`;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    //add language
//                    ldb.ExecuteStoreCommand(@"ALTER TABLE  `language`ADD UNIQUE (`language_description`);");
//                    lsql = @"INSERT INTO language (`language_description`) 
//                             SELECT distinct c.`Language` 
//                             FROM csv as c
//                             ON DUPLICATE KEY UPDATE `language_description`= c.`Language`;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    //add genres
//                    ldb.ExecuteStoreCommand(@"ALTER TABLE  `genre`ADD UNIQUE (`genre_description`);");
//                    lsql = @"INSERT INTO genre (`genre_description`) 
//                             SELECT distinct c.`Genres` 
//                             FROM csv as c
//                             ON DUPLICATE KEY UPDATE `genre_description`= c.`Genres`;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    //add publisher
//                    ldb.ExecuteStoreCommand(@"ALTER TABLE  `publisherforsong`ADD UNIQUE (`publisherforsong_description`);");
//                    lsql = @"INSERT INTO publisher (`publisherforsong_description`) 
//                             SELECT distinct c.`Publisher` 
//                             FROM csv as c
//                             ON DUPLICATE KEY UPDATE `publisherforsong_description`= c.`Publisher`;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    //add song
//                    lsql = @"INSERT INTO `song`(`song_originName`, `song_engName`, `song_genre`, `song_languageId`, `song_albumId`, `song_artistId`, `song_length`, `song_lyrics`, `publisherforsong_id`, `song_picture`, `song_status`, `song_addedDate`, `song_price`, `song_URL_iOS`, `song_URL_Android/Other`, `song_URL_RTMP`, `song_Copyright`, `song_Track Number`,`song_releasedDate`)   
//                             SELECT distinct c.`Title - TH`,c.`Title - EN`,
//                             (select genre_id from genre where genre_description = c.Genres),
//                             (select language_id from language where language_description = c.Language),
//                             (select album_id from album where album_description_th = c.`Album - TH`),
//                             (select artist_id from artist where artist_description_th = c.`Artist - TH`),
//                             c.Length,c.Lyrics,
//                             (select publisherforsong_id from publisherforsong where publisherforsong_description = c.`Publisher`),
//                             c.Photo,1,now(),c.Price,c.`URL iOS`, c.`URL Android/Other`, c.`URL RTMP`, c.`Copyright`, c.`Track Number`,now()
//                             FROM csv as c;";
//                    ldb.ExecuteStoreCommand(lsql);

//                    return "Export complete";
                    
//                }
//            }

//            return "tester";
//        }

        public ActionResult Importdata()
        {
            return View();
        }

        /// <summary>
        /// import step 1 => upload file and insert to csv
        /// </summary>
        /// <returns>status</returns>
        [HttpPost]
        public string importstep1()
        {

            if (Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/import/"), fileName);
                        file.SaveAs(path);

                        singspazeEntities ldb = new singspazeEntities();
                        var filepath = Path.Combine(Server.MapPath("~/Picture/song"));
                        var address = Request.ServerVariables["LOCAL_ADDR"] + ":" + Request.ServerVariables["server_port"] + @"\Picture\song";
                        //var sql = "";
                        //if(Request.Form.GetValues("encode")[0] == "tis620")
                        var sql = @"LOAD DATA  INFILE  '" + path.Replace(@"\", @"\\") + @"' IGNORE INTO TABLE  `csv` CHARACTER SET '" + Request.Form.GetValues("encode")[0] + @"' FIELDS TERMINATED BY  ',' ENCLOSED BY  '" + '"' + "' ESCAPED BY  '" + @"\\" + "' LINES TERMINATED BY  '" + @"\r\n" + "' IGNORE 1 LINES;";
                        //else
                        //    sql = @"LOAD DATA  INFILE  '" + path.Replace(@"\", @"\\") + @"' IGNORE INTO TABLE  `csv` CHARACTER SET 'utf8' FIELDS TERMINATED BY  ',' ENCLOSED BY  '" + '"' + "' ESCAPED BY  '" + @"\\" + "' LINES TERMINATED BY  '" + @"\r\n" + "' IGNORE 1 LINES;";
                        ldb.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;");
                        ldb.Database.ExecuteSqlCommand("TRUNCATE TABLE  `csv`");
                        ldb.Database.ExecuteSqlCommand(sql);

                        ldb.Database.ExecuteSqlCommand(@"UPDATE `csv` SET `Photo`= concat('" + address.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ',''));");
                        ldb.Database.ExecuteSqlCommand(@"UPDATE `csv` SET `Lyrics`= concat(concat('" + filepath.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ','')),'\\lyrics.txt');");

                        //ldb.ExecuteStoreCommand("SET SQL_SAFE_UPDATES = 0;");
                        //ldb.ExecuteStoreCommand("TRUNCATE TABLE  `csv`");
                        //ldb.ExecuteStoreCommand(sql);

                        //ldb.ExecuteStoreCommand(@"UPDATE `csv` SET `Photo`= concat('" + address.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ',''));");
                        //ldb.ExecuteStoreCommand(@"UPDATE `csv` SET `Lyrics`= concat(concat('" + filepath.Replace(@"\", @"\\") + @"\\" + @"',REPLACE(`Title - EN`,' ','')),'\\lyrics.txt');");

                        return "Success";
                    }
                    else
                        return "No data or format";
                    
                }
                catch (Exception e)
                {
                    return "Error";
                }
            }
            else
                return "No file data";
            
            
        }

        /// <summary>
        /// import step 2 => update Lyrics data in csv
        /// </summary>
        /// <returns>status</returns>
        public string importstep2()
        {
            try
            {

                singspazeEntities ldb = new singspazeEntities();
                //Lyrics
                string lyrics = "";
                string lsql = "";
                List<string> resultscsv = ldb.Database.SqlQuery<string>(@"select Lyrics from csv;").ToList();
                foreach (string data in resultscsv)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(data.Replace(@"\", @"\\")))
                        {
                            try
                            {
                                lyrics = sr.ReadToEnd();

                                lsql = lsql + @"UPDATE `csv` SET `Lyrics`= '" + lyrics + "' where `Lyrics`='" + data.Replace(@"\", @"\\").Replace(@"'",@"\'") + "';";
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        lsql = lsql + @"UPDATE `csv` SET `Lyrics`= '' where `Lyrics`='" + data.Replace(@"\", @"\\").Replace(@"'", @"\'") + "';";
                    }

                }
                ldb.Database.ExecuteSqlCommand(lsql);

                //datetime
                ldb.Database.ExecuteSqlCommand(@"UPDATE `csv` SET `Released date`= DATE_FORMAT(NOW(), '%d/%m/%Y %T') where `Released date`='';");
                return "Success";
            }
            catch (Exception e)
            {
                return e.ToString();
                return "Error";
            }


        }

        /// <summary>
        /// import step 3 => add new artist album language genres publisher
        /// </summary>
        /// <returns>status</returns>
        public string importstep3()
        {
            //try
            //{

                singspazeEntities ldb = new singspazeEntities();
                var address = Request.ServerVariables["LOCAL_ADDR"] + ":" + Request.ServerVariables["server_port"] + @"\Picture\artist";
                string lsql = "";
                //add artist
                //ldb.ExecuteStoreCommand(@"ALTER TABLE  `artist`ADD UNIQUE (`artist_description_th`,`artist_description_en`);");
                lsql = @"INSERT INTO artist (`artist_description_th`,`artist_description_en`,`artist_view`,`ArtistPicture_L_Lo`, `ArtistPicture_L_Hi`, `ArtistPicture_S_Lo`, `ArtistPicture_S_Hi`) 
                             SELECT distinct c.`Artist - TH`, c.`Artist - EN`,0,'newartist','newartist','newartist','newartist'
                             FROM csv as c
                             ON DUPLICATE KEY UPDATE `artist_description_th`= c.`Artist - TH`,`artist_description_en`= c.`Artist - EN`;";
                ldb.Database.ExecuteSqlCommand(lsql);
                address = address.Replace(@"\", @"/");
                //update artist
                lsql = @"UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/00000' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/00000' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/00000' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/00000' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`< 10 and `ArtistPicture_L_Lo`= 'newartist';

                        UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/0000' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/0000' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/0000' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/0000' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`> 9 and `artist_id` < 100 and `ArtistPicture_L_Lo`= 'newartist';
                        
                        UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/000' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/000' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/000' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/000' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`> 99 and `artist_id` < 1000 and `ArtistPicture_L_Lo`= 'newartist';

                        UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/00' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/00' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/00' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/00' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`> 999 and `artist_id` < 10000 and `ArtistPicture_L_Lo`= 'newartist';

                        UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/0' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/0' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/0' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/0' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`> 9999 and `artist_id` < 100000 and `ArtistPicture_L_Lo`= 'newartist';
                        
                        UPDATE `artist` SET 
                        `ArtistPicture_L_Lo`= concat('" + address + @"', '/' , `artist_id` ,  '_cover_L_1.jpg'),
                        `ArtistPicture_L_Hi`=concat('" + address + @"' , '/' , `artist_id` , '_cover_L_2.jpg'),
                        `ArtistPicture_S_Lo`=concat('" + address + @"', '/' , `artist_id` , '_cover_s_01.jpg') ,
                        `ArtistPicture_S_Hi`=concat('" + address + @"', '/' , `artist_id` , '_cover_s_02.jpg')
                        where `artist_id`> 99999 and `artist_id` < 1000999 and `ArtistPicture_L_Lo`= 'newartist';
                        ";
                ldb.Database.ExecuteSqlCommand(lsql);
                //add album
                //ldb.ExecuteStoreCommand(@"ALTER TABLE  `album`ADD UNIQUE (`album_description_th`,`album_description_en`);");
                lsql = @"INSERT INTO album (`album_description_th`,`album_description_en`) 
                             SELECT distinct c.`Album - TH`, c.`Album - EN` 
                             FROM csv as c
                             ON DUPLICATE KEY UPDATE `album_description_th`= c.`Album - TH`,`album_description_en`= c.`Album - EN`;";
                ldb.Database.ExecuteSqlCommand(lsql);

                //add language
                //ldb.ExecuteStoreCommand(@"ALTER TABLE  `language`ADD UNIQUE (`language_description`);");
                lsql = @"INSERT INTO language (`language_description`) 
                             SELECT distinct c.`Language` 
                             FROM csv as c
                             ON DUPLICATE KEY UPDATE `language_description`= c.`Language`;";
                ldb.Database.ExecuteSqlCommand(lsql);

                //add genres
                //ldb.ExecuteStoreCommand(@"ALTER TABLE  `genre`ADD UNIQUE (`genre_description`);");
                lsql = @"INSERT INTO genre (`genre_description`) 
                             SELECT distinct c.`Genres` 
                             FROM csv as c
                             ON DUPLICATE KEY UPDATE `genre_description`= c.`Genres`;";
                ldb.Database.ExecuteSqlCommand(lsql);

                //add publisher
                //ldb.ExecuteStoreCommand(@"ALTER TABLE  `publisherforsong`ADD UNIQUE (`publisherforsong_description`);");
                lsql = @"INSERT INTO publisherforsong (`publisherforsong_description`) 
                             SELECT distinct c.`Publisher` 
                             FROM csv as c
                             ON DUPLICATE KEY UPDATE `publisherforsong_description`= c.`Publisher`;";
                ldb.Database.ExecuteSqlCommand(lsql);              
                
                    


                return "Success";
            //}
            //catch (Exception e)
            //{
            //    return "Error";
            //}


        }

        /// <summary>
        /// import step 4 =>  update and add song
        /// </summary>
        /// <returns>status</returns>
        public string importstep4()
        {
            try
            {

            singspazeEntities ldb = new singspazeEntities();

            string lsql = "";
            //update song
            lsql = @"SET SQL_SAFE_UPDATES = 0;UPDATE  song s INNER JOIN  csv c ON  s.song_id = c.id SET
                        s.song_originName = c.`Title - TH`,
                        s.song_engName = c.`Title - EN`,
                        s.song_genre = (select genre_id from genre where genre_description = c.Genres),
                        s.song_languageId = (select language_id from language where language_description = c.Language),
                        s.song_albumId = (select album_id from album where album_description_th = c.`Album - TH`),
                        s.song_artistId = (select artist_id from artist where artist_description_th = c.`Artist - TH`),
                        s.song_length = c.Length,
                        s.song_status = c.Status,
                        s.song_lyrics = c.Lyrics,
                        s.publisherforsong_id = (select publisherforsong_id from publisherforsong where publisherforsong_description = c.`Publisher`),
                        s.song_URL_picture = c.Photo,
                        s.song_addedDate = now(),
                        s.song_price = CAST(c.Price AS DECIMAL(12,2)),
                        s.song_view = 0,
                        s.song_URL_iOS = c.`URL iOS`,
                        s.`song_URL_Android_Other` = c.`URL Android/Other`,
                        s.song_URL_RTMP = c.`URL RTMP`,
                        s.song_Copyright = c.`Copyright`,
                        s.song_Track_Number = c.`Track Number`,
                        s.song_releasedDate = STR_TO_DATE(c.`Released date`,'%d/%m/%Y %T');";
            ldb.Database.ExecuteSqlCommand(lsql);

            //add new song
            lsql = @"INSERT INTO `song`(`song_originName`, `song_engName`, `song_genre`, `song_languageId`, `song_albumId`, `song_artistId`, `song_length`, `song_lyrics`, `publisherforsong_id`, `song_URL_picture`, `song_status`, `song_addedDate`, `song_price`, `song_URL_iOS`, `song_URL_Android_Other`, `song_URL_RTMP`, `song_Copyright`, `song_Track_Number`,`song_releasedDate`,`song_view`)   
                             SELECT distinct c.`Title - TH`,c.`Title - EN`,
                             (select genre_id from genre where genre_description = c.Genres),
                             (select language_id from language where language_description = c.Language),
                             (select album_id from album where album_description_th = c.`Album - TH`),
                             (select artist_id from artist where artist_description_th = c.`Artist - TH`),
                             c.Length,c.Lyrics,
                             (select publisherforsong_id from publisherforsong where publisherforsong_description = c.`Publisher`),
                             c.Photo,c.Status,now(),CAST(c.Price AS DECIMAL(12,2)),c.`URL iOS`, c.`URL Android/Other`, c.`URL RTMP`, c.`Copyright`, c.`Track Number`,STR_TO_DATE(c.`Released date`,'%d/%m/%Y %T'),0
                             FROM csv as c where c.id = 0;";
            ldb.Database.ExecuteSqlCommand(lsql);




            return "Success";
            }
            catch (Exception e)
            {
                return "Error";
            }


        }


        public void ExportCSV()
        {

            StringWriter sw = new StringWriter();

            //sw.WriteLine("\"id\",\"Title - TH\",\"Title - EN\",\"Photo\",\"Artist - TH\",\"Artist - EN\",\"Length\",\"Lyrics\",\"Album - TH\",\"Album - EN\",\"Released date\",\"#Views (today, 30 days, total)\",\"Language\",\"Genres\",\"Publisher\",\"Status\",\"Price\",\"Copyright\",\"Track Number\",\"URL iOS\",\"URL Android/Other\",\"URL RTMP\"");
            sw.WriteLine("id,Title - TH,Title - EN,Photo,Artist - TH,Artist - EN,Length,Lyrics,Album - TH,Album - EN,Released date,\"#Views (today, 30 days, total)\",Language,Genres,Publisher,Status,Price,Copyright,Track Number,URL iOS,URL Android/Other,URL RTMP");
            																					

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Export.csv");
            //Response.ContentType = "application/excel";
            Response.ContentType = "application/octet-stream";            ;
            //Response.Charset = "UTF-8";
            Response.Charset = "TIS-620";
            //Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");
            

            List<CSVList> csvlist = new List<CSVList>
            {
                 //new CSVList ( "Adam",  "Bielecki",  DateTime.Parse("22/05/1986"),       "adamb@example.com" ), 
                 //new CSVList (  "George", "Smith",  DateTime.Parse("10/10/1990"),  "george@example.com" )
            };
            csvlist = (from x in db.song
                       select new CSVList
                           {
                               id = x.song_id,
                               originname = x.song_originName,
                               engname = x.song_engName,
                               photo = "",
                               artistTH = db.artist.FirstOrDefault(art => art.artist_id == x.song_artistId).artist_description_th,
                               artistEN = db.artist.FirstOrDefault(art => art.artist_id == x.song_artistId).artist_description_en,
                               length = x.song_length,
                               lyrics = "",
                               albumTH = db.album.FirstOrDefault(a => a.album_id == x.song_albumId).album_description_th,
                               albumEN = db.album.FirstOrDefault(a => a.album_id == x.song_albumId).album_description_en,
                               released = x.song_releasedDate,
                               views = "",
                               language = db.language.FirstOrDefault(l => l.language_id == x.song_languageId).language_description,
                               genres = db.genre.FirstOrDefault(g => g.genre_id == x.song_genre).genre_description,
                               publisher = db.publisherforsong.FirstOrDefault(ps => ps.publisherforsong_Id == x.publisherforsong_id).publisherforsong_description,
                               status = x.song_status,
                               price = x.song_price,
                               copyright = x.song_Copyright,
                               tracknumber = x.song_Track_Number,
                               url_iOS = x.song_URL_iOS,
                               url_Android = x.song_URL_Android_Other,
                               url_RTMP = x.song_URL_RTMP

                               //Id = x.song_id,
                               //Name = x.song_originName,
                               //Album = db.album.FirstOrDefault(a => a.album_id == x.song_albumId).album_description_th,
                               //Artist = db.artist.FirstOrDefault(ar => ar.artist_id == x.song_artistId).artist_description_th,
                               //Language = db.language.FirstOrDefault(l => l.language_id == x.song_languageId).language_description,
                               //Genre = db.genre.FirstOrDefault(g => g.genre_id == x.song_genre).genre_description,
                               //Publisher = db.publisherforsong.FirstOrDefault(p => p.publisherforsong_Id == x.publisherforsong_id).publisherforsong_description,
                               //Length = x.song_length,
                               //Status = x.song_status
                           }).OrderBy(x => x.id).ToList();

            foreach (var line in csvlist)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\",\"{19}\",\"{20}\",\"{21}\"",
                                           line.id,
                                           line.originname,
                                           line.engname,
                                           line.photo,
                                           line.artistTH,
                                           line.artistEN,
                                           line.length,
                                           line.lyrics,
                                           line.albumTH,
                                           line.albumEN,
                                           line.released.ToString("d/M/yyyy", CultureInfo.InvariantCulture),
                                           line.views,
                                           line.language,
                                           line.genres,
                                           line.publisher,
                                           line.status,
                                           line.price,
                                           line.copyright,
                                           line.tracknumber,
                                           line.url_iOS,
                                           line.url_Android,
                                           line.url_RTMP));

                //sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}",
                //                           line.id,
                //                           line.originname,
                //                           line.engname,
                //                           line.photo,
                //                           line.artistTH,
                //                           line.artistEN,
                //                           line.length,
                //                           line.lyrics,
                //                           line.albumTH,
                //                           line.albumEN,
                //                           line.released,
                //                           line.views,
                //                           line.language,
                //                           line.genres,
                //                           line.publisher,
                //                           line.status,
                //                           line.price,
                //                           line.copyright,
                //                           line.tracknumber,
                //                           line.url_iOS,
                //                           line.url_Android,
                //                           line.url_RTMP));
            }

            byte[] BOM = new byte[] { 0xef, 0xbb, 0xbf };
            Response.BinaryWrite(BOM);//write the BOM first

            //Response.BinaryWrite(Encoding.GetEncoding("TIS-620").GetBytes(sw.ToString()));
            Response.BinaryWrite(Encoding.UTF8.GetBytes(sw.ToString()));
            //Response.Write(sw.ToString());

            Response.End();

        }


        public ActionResult CSV(int id = 0)
        {
            List<csv> csvdata = db.csv.ToList();
            if (csvdata == null)
            {
                return HttpNotFound();
            }
            return View(csvdata);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}