using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SingSpaze.Models;
using SingSpaze.Models.Input;
using SingSpaze.Models.Output;

namespace SingSpaze.Controllers.API
{
    public class SongController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        
        //[Authorize]
        [HttpPost]
        [ActionName("List")]
        public O_SongList SongList(I_SongList i_data)
        {
            if (i_data == null)
            {
                return new O_SongList()
                {
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            List<song> listsong = new List<song>();

            var before = DateTime.Now.AddDays(-i_data.time);

            // order
            if (string.IsNullOrEmpty(i_data.type))
                listsong = db.song.OrderByDescending(s => s.song_id).ToList();
            else if (i_data.type == "new")
                listsong = db.song.Where(s => s.song_releasedDate > before).OrderByDescending(s => s.song_releasedDate).ToList();
            else if (i_data.type == "hot")
            {
               var grouphistorysong = from history in db.singinghistory
                                      where history.singinghistory_date > before
                                      group history by history.song_id into ghistory
                                      select new { song_id = ghistory.Key, count = ghistory.Count()};

                var joinhistory = from dbsong in db.song.AsEnumerable()
                                  join history in grouphistorysong.AsEnumerable()
                                  on dbsong.song_id equals history.song_id
                                  orderby history.count descending
                                  select dbsong;
                                  //select new { dbsong,count = history.count}
                
                listsong = joinhistory.ToList();
                
            }
            //else if (i_data.type == "recommend")
            //    listsong = db.song.ToList();

            
            // where 
            if (i_data.genre_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.genre_id).Contains(s.song_genre)).ToList();
            if (i_data.artist_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.artist_id).Contains(s.song_artistId)).ToList();
            if (i_data.album_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.album_id).Contains(s.song_albumId)).ToList();
            if (i_data.language_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.language_id).Contains(s.song_languageId)).ToList();

           
            // skip take
            listsong = listsong.Skip(i_data.selectdata.skip).Take(i_data.selectdata.take).ToList();

            if (listsong == null)
            {
                return new O_SongList()
                {
                    errordata = new errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<listsongdata> o_song = new List<listsongdata>();

            foreach (song data in listsong)
            {
                o_song.Add(new listsongdata()
                    {
                        id = data.song_id,
                        originName = data.song_originName,
                        engName = data.song_engName,
                        price = data.song_price,
                        thumbnail = data.song_thumbnail,
                        picture = data.song_picture,
                        view = data.song_view,

                        languagedata = Useful.getlanguagedata(data.song_languageId),
                        albumdata = Useful.getalbumdata(data.song_albumId),
                        artistdata = Useful.getartistdata(data.song_artistId),
                        genredata = Useful.getgenredata(data.song_genre),
                        recordlabeldata = Useful.getrecordlabeldata(data.song_recordLabelId),
                        contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                    });
            }

            return new O_SongList()
            {
                listsong = o_song   
            };
        }

        
        //[Authorize]
        [HttpPost]
        [ActionName("Sing")]
        public O_PlaySong PlaySong(int id,I_PlaySong i_data)
        {
            if (i_data == null)
            {
                return new O_PlaySong()
                {
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_PlaySong()
                       {
                           errordata = new errordata()
                           {
                               code = 5,
                               Detail = Useful.geterrordata(5)
                           }
                       };
            }

            song datasong = db.song.FirstOrDefault(u => u.song_id == id);

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, datauser);

            if (datasong == null)
            {
                return new O_PlaySong()
                {
                    errordata = new errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
                
            }

            //update song
            datasong.song_view = datasong.song_view + 1;

            //add singinghistory
            singinghistory history = new singinghistory()
            {
                song_id = id,
                user_id = i_data.logindata.id,
                artist_id = datasong.song_artistId,
                singinghistory_date = DateTime.Now
            };
            db.singinghistory.AddObject(history);

            //save
            db.SaveChanges();
            

            songdata data = new songdata()
            {
                id = datasong.song_id,
                engName = datasong.song_engName,
                originName = datasong.song_originName,
                lyrics = datasong.song_lyrics,
                picture = datasong.song_picture,
                price = datasong.song_price,
                releasedDate = datasong.song_releasedDate,
                thumbnail = datasong.song_thumbnail,
                view = datasong.song_view,
                filePath = datasong.song_filePath,
                length = datasong.song_length,
                keywords = datasong.song_keywords,

                languagedata = Useful.getlanguagedata(datasong.song_languageId),
                albumdata = Useful.getalbumdata(datasong.song_albumId),
                artistdata = Useful.getartistdata(datasong.song_artistId),
                genredata = Useful.getgenredata(datasong.song_genre),
                recordlabeldata = Useful.getrecordlabeldata(datasong.song_recordLabelId),
                contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId)
                
            };

            
            return new O_PlaySong()
                {
                  songdata = data  
                };
            
        }


        //[Authorize]
        //[HttpPost]
        //[ActionName("Song")]
        //public O_PlaySong playsong(int id, I_PlaySong i_data) //song_id
        //{
        //    if (!Useful.checklogin(i_data.logindata))
        //    {
        //        return new O_PlaySong()
        //        {
        //            errordata = new errordata()
        //            {
        //                code = 5,
        //                Detail = Useful.geterrordata(5)
        //            }
        //        };
        //    }

        //    song datasong = db.song.FirstOrDefault(u => u.song_id == id);

        //    //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, datauser);

        //    if (datasong == null)
        //    {
        //        return new O_PlaySong()
        //        {
        //            errordata = new errordata()
        //            {
        //                code = 6,
        //                Detail = Useful.geterrordata(6)
        //            }
        //        };

        //    }

        //    singinghistory history = new singinghistory()
        //    {
        //        song_id = id,
        //        user_id = i_data.logindata.id,
        //        singinghistory_date = DateTime.Now
        //    };
        //    db.singinghistory.AddObject(history);
        //    db.SaveChanges();

        //    return new O_PlaySong()
        //    {
        //        filepath = datasong.song_filePathId.ToString()
        //    };

        //}

        
    }
}