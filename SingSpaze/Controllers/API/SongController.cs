﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SingSpaze.Models;
using SingSpaze.Models.Input;
using SingSpaze.Models.Output;
using System.Security.Cryptography;
using System.Web;
using System.Text;

namespace SingSpaze.Controllers.API
{
    /// <summary>
    /// Song api 
    /// </summary>
    public class SongController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        
        /// <summary>
        /// For show listsong about hot song,new song
        /// </summary>
        /// <param name="i_data">Class I_SongList</param>
        /// <returns>Class O_SongList</returns>
        [HttpPost]
        [ActionName("SongList")]
        public O_SongList SongList(I_SongList i_data)
        {
            if (i_data == null)
            {
                return new O_SongList()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            List<song> listsong = new List<song>();

            var before = DateTime.Now.AddDays(-i_data.time);
            var grouphistorysong = from history in db.viewhistory
                                   where history.ViewHistory_Date > before
                                   group history by history.Song_Id into ghistory
                                   select new { song_id = ghistory.Key, count = ghistory.Count() };

            // order
            if (string.IsNullOrEmpty(i_data.type))
                listsong = db.song.OrderBy(s => Encoding.GetEncoding("tis-620").GetString(Encoding.Default.GetBytes(s.song_originName))).ToList();
            else if (i_data.type.ToLower() == "new")
                listsong = db.song.Where(s => s.song_releasedDate > before).OrderByDescending(s => s.song_releasedDate).ToList();
            else if (i_data.type.ToLower() == "hot")
            {
               //var grouphistorysong = from history in db.singinghistory
               //                        where history.singinghistory_date > before
               //                        group history by history.song_id into ghistory
               //                        select new { song_id = ghistory.Key, count = ghistory.Count() };

               

               var joinhistory = from dbsong in db.song.AsEnumerable()
                                  join history in grouphistorysong.AsEnumerable()
                                  on dbsong.song_id equals history.song_id
                                  orderby history.count descending
                                  select dbsong;
                                  //select new { dbsong,count = history.count}
                
                listsong = joinhistory.ToList();
                
            }
            else
                listsong = db.song.OrderBy(s => Encoding.GetEncoding("tis-620").GetString(Encoding.Default.GetBytes(s.song_originName))).ToList();
            //else if (i_data.type == "recommend")
            //    listsong = db.song.ToList();

            //active and langage
            listsong = listsong.Where(s => s.song_status == 1 && s.song_languageId == i_data.language_id).ToList();


            // where 
            //if (i_data.genre_id != null)
            //    listsong = listsong.Where(s => Useful.getlistdata(i_data.genre_id).Contains(s.song_genre)).ToList();
            if (i_data.artist_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.artist_id).Contains(s.song_artistId)).ToList();
            if (i_data.album_id != null)
                listsong = listsong.Where(s => Useful.getlistdata(i_data.album_id).Contains(s.song_albumId)).ToList();
            //if (i_data.language_id != null)
            //    listsong = listsong.Where(s => Useful.getlistdata(i_data.language_id).Contains(s.song_languageId)).ToList();
            if(i_data.categories != null)
            {
                List<int> categories_id = new List<int>();
                if(i_data.categories.ToLower() == "others")
                {
                    //others
                    //categories_id = db.genre.Select(g => g.genre_id).ToList();
                    string data = "pop,rock,classic,luktung";
                    List<string> others = new List<string>();
                    string[] arraydata = data.Split(',');

                    foreach (string stringdata in arraydata)
                        others.Add(stringdata);

                    categories_id = db.genre.Where(g => others.Contains(g.genre_description.ToLower())).Select(g => g.genre_id).ToList();
                    listsong = listsong.Where(s => !categories_id.Contains(s.song_genre)).ToList();
                }else
                {
                    categories_id = db.genre.Where(g => g.genre_description.ToLower() == i_data.categories.ToLower()).Select(g => g.genre_id).ToList();
                    if (categories_id.Count != 0)
                        listsong = listsong.Where(s => categories_id.Contains(s.song_genre)).ToList();
                }
                
            }

            int resultNumber = listsong.Count();
            // skip take
            listsong = listsong.Skip(i_data.selectdata.startindex-1).Take(i_data.selectdata.endindex-i_data.selectdata.startindex+1).ToList();

            if (listsong == null)
            {
                return new O_SongList()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<Songdata> o_song = new List<Songdata>();

            foreach (song data in listsong)
            {
                int view = Useful.getview(data.song_id); //all time
                if (i_data.type == "hot")
                    view = grouphistorysong.Where(s => s.song_id == data.song_id).Select(s => s.count).SingleOrDefault();
                
                o_song.Add(Useful.getsongdata(data.song_id,view));
                //o_song.Add(new Songdata()
                //    {
                //        id = data.song_id,
                //        originName = data.song_originName,
                //        engName = data.song_engName,
                //        price = data.song_price,
                //        //thumbnail = data.song_thumbnail,
                //        URL_picture = data.song_URL_picture,
                //        view = view,
                //        length = data.song_length,
                //        releasedDate = data.song_releasedDate,

                //        //languagedata = Useful.getlanguagedata(data.song_languageId),
                //        albumdata = Useful.getalbumdata(data.song_albumId),
                //        artistdata = Useful.getartistdata(data.song_artistId),
                //        genredata = Useful.getgenredata(data.song_genre),
                //        publisherdata = Useful.getpublishersongdata(data.publisherforsong_id),
                //        //contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                //    });
            }

            return new O_SongList()
            {
                resultNumber = resultNumber,
                listsong = o_song.ToList()   
            };
        }

        
        /// <summary>
        /// For get song url and update viewdata
        /// </summary>
        /// <param name="i_data">Class I_PlaySong</param>
        /// <returns>Class O_PlaySong</returns>
        [HttpPost]
        [ActionName("PlaySong")]
        public O_PlaySong PlaySong(I_PlaySong i_data)
        {
            if (i_data == null)
            {
                return new O_PlaySong()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_PlaySong()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }
            int user_id = Useful.getuserid(i_data.logindata.token);
            song datasong = db.song.FirstOrDefault(u => u.song_id == i_data.id);

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, datauser);

            if (datasong == null)
            {
                return new O_PlaySong()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
                
            }

            //update song
            //datasong.song_view = datasong.song_view + 1;
            //update artist
            //artist dataartist = db.artist.FirstOrDefault(a => a.artist_id == datasong.song_artistId);
            //dataartist.artist_view = dataartist.artist_view + 1;

            //add singinghistory
            singinghistory singhistory = new singinghistory()
            {
                song_id = i_data.id,
                user_id = user_id,
                artist_id = datasong.song_artistId,
                singinghistory_date = DateTime.Now
            };
            db.singinghistory.AddObject(singhistory);

            viewhistory lastview = db.viewhistory.Where(v =>  v.Song_Id == i_data.id).OrderByDescending(v => v.ViewHistory_Date).FirstOrDefault();
            //DateTime lastview = view.ViewHistory_Date;

            if (lastview == null || lastview.ViewHistory_Date.Day != DateTime.Now.Day || lastview.ViewHistory_Date.Month != DateTime.Now.Month)
            {
                //add viewhistory
                viewhistory viewhistory = new viewhistory()
                {
                    Song_Id = i_data.id,
                    User_Id = user_id,
                    ViewHistory_Date = DateTime.Now
                };
                db.viewhistory.AddObject(viewhistory);
            }

            //add WTBtoken

            string Token = Useful.GetMd5Hash(MD5.Create(), DateTime.Now.ToString() + i_data.id.ToString() + user_id.ToString() + "song");
            wtbtokens wtbtokens = new wtbtokens()
            {
                user_id = user_id,
                WTBTokens_token = Token,
                WTBTokens_ipaddress = HttpContext.Current.Request.UserHostAddress,
                //WTBTokens_timestamp = DateTime.Now
            };
            wtbtokens lasttoken = db.wtbtokens.Where(w => w.user_id == user_id).FirstOrDefault();
            if (lasttoken != null)
            {
                lasttoken.user_id = wtbtokens.user_id;
                lasttoken.WTBTokens_token = wtbtokens.WTBTokens_token;
                lasttoken.WTBTokens_ipaddress = wtbtokens.WTBTokens_ipaddress;
            }
            else
                db.wtbtokens.AddObject(wtbtokens);
            //save
            db.SaveChanges();
            

            //Songdata data = new Songdata()
            //{

            //    id = datasong.song_id,
            //    engName = datasong.song_engName,
            //    originName = datasong.song_originName,
            //    lyrics = datasong.song_lyrics,
            //    URL_picture = datasong.song_URL_picture,
            //    price = datasong.song_price,
            //    releasedDate = datasong.song_releasedDate,
            //    //thumbnail = datasong.song_thumbnail,
            //    view = Useful.getview(datasong.song_id),
            //    //filePath = datasong.song_filePath,
            //    length = datasong.song_length,
            //    //keywords = datasong.song_keywords,

            //    //url
            //    url_iOS = datasong.song_URL_iOS + "?token=" +Token,
            //    url_Android_Other = datasong.song_URL_Android_Other + "?token=" + Token,
            //    url_RTMP = datasong.song_URL_RTMP + "?token=" + Token,

            //    //data
            //    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
            //    albumdata = Useful.getalbumdata(datasong.song_albumId),
            //    artistdata = Useful.getartistdata(datasong.song_artistId),
            //    genredata = Useful.getgenredata(datasong.song_genre),
            //    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
            //    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId)
                
            //};

            
            return new O_PlaySong()
                {
                    songdata = Useful.getsongdata(datasong.song_id,0, Token)  
                };
            
        }

        /// <summary>
        /// For searchsong with keyword
        /// </summary>
        /// <param name="i_data">Class I_SearchSong</param>
        /// <returns>Class O_SearchSong</returns>
        [HttpPost]
        [ActionName("SearchSong")]
        public O_SearchSong SearchSong(I_SearchSong i_data)
        {
            if (i_data == null)
            {
                return new O_SearchSong()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (i_data.keyword == null)
                i_data.keyword = "";

            List<song> listsong = new List<song>();

            listsong = (from dbsong in db.song.ToList()
                       join dbartist in db.artist.ToList()
                       on dbsong.song_artistId equals dbartist.artist_id
                       join dbalbum in db.album.ToList()
                       on dbsong.song_albumId equals dbalbum.album_id
                       where dbsong.song_status == 1 && dbsong.song_languageId == i_data.language_id
                       orderby dbsong.song_originName ascending
                       select dbsong).ToList();
                       


            // where 
            if (i_data.type != null)
            {
                if (i_data.type.ToLower() == "artist")
                    listsong = listsong.Where(s => Useful.getartistdata(s.song_artistId).description_TH.Contains(i_data.keyword)).ToList();
                else if (i_data.type.ToLower() == "album")
                    listsong = listsong.Where(s => Useful.getalbumdata(s.song_albumId).description_TH.Contains(i_data.keyword)).ToList();
                else if (i_data.type.ToLower() == "lyrics")
                    listsong = listsong.Where(s => s.song_lyrics.Contains(i_data.keyword)).ToList();
                else if (i_data.type.ToLower() == "song name")
                    listsong = listsong.Where(s => s.song_originName.Contains(i_data.keyword) || s.song_engName.ToLower().Contains(i_data.keyword.ToLower())).ToList();
            }
            else //null will be song name
                listsong = listsong.Where(s => s.song_originName.Contains(i_data.keyword) || s.song_engName.ToLower().Contains(i_data.keyword.ToLower())).ToList();
            
                    

            int resultNumber = listsong.Count();

            
            // skip take
            
            listsong = listsong.OrderBy(l => Encoding.GetEncoding("tis-620").GetString(Encoding.Default.GetBytes(l.song_originName))).Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList();
            //listsong = listsong.OrderBy(l => l.song_originName).Skip(i_data.selectdata.startindex-1).Take(i_data.selectdata.endindex-i_data.selectdata.startindex+1).ToList();

            if (listsong == null)
            {
                return new O_SearchSong()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<Songdata> o_song = new List<Songdata>();

            foreach (song data in listsong)
            {
                o_song.Add(Useful.getsongdata(data.song_id));
                //o_song.Add(new Songdata()
                //{
                //    id = data.song_id,
                //    originName = data.song_originName,
                //    engName = data.song_engName,
                //    price = data.song_price,
                //    //thumbnail = data.song_thumbnail,
                //    URL_picture = data.song_URL_picture,
                //    view = Useful.getview(data.song_id),
                //    length = data.song_length,

                //    //languagedata = Useful.getlanguagedata(data.song_languageId),
                //    albumdata = Useful.getalbumdata(data.song_albumId),
                //    artistdata = Useful.getartistdata(data.song_artistId),
                //    genredata = Useful.getgenredata(data.song_genre),
                //    publisherdata = Useful.getpublishersongdata(data.publisherforsong_id),
                //    //contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                //});
            }

            return new O_SearchSong()
            {
                listsong = o_song,
                resultNumber = resultNumber
            };
        }


        /// <summary>
        /// Get your singing history
        /// </summary>
        /// <param name="i_data">Class I_SearchSong</param>
        /// <returns>Class O_SearchSong</returns>
        [HttpPost]
        [ActionName("GetSingingHistory")]
        public O_SingHistory GetSingingHistory(I_SingHistory i_data)
        {
            if (i_data == null)
            {
                return new O_SingHistory()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_SingHistory()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }


            int user_id = Useful.getuserid(i_data.logindata.token);
            var before = DateTime.Now.AddDays(-i_data.time);

            List<singinghistory> listhistory = db.singinghistory.Where(h => h.user_id == user_id && h.singinghistory_date > before).ToList();

            // skip take
            listhistory = listhistory.Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList();


            if (listhistory == null)
            {
                return new O_SingHistory()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<Singhistorydata> o_singhistorydata = new List<Singhistorydata>();

            foreach (singinghistory data in listhistory)
            {
                song song = db.song.FirstOrDefault(s => s.song_id == data.song_id);
                Songdata songdata = new Songdata()
                {
                    id = song.song_id,
                    originName = song.song_originName,
                    engName = song.song_engName,
                    price = song.song_price,
                    //thumbnail = data.song_thumbnail,
                    URL_picture = song.song_URL_picture,
                    view = Useful.getview(data.song_id),
                    length = song.song_length,

                    //languagedata = Useful.getlanguagedata(data.song_languageId),
                    albumdata = Useful.getalbumdata(song.song_albumId),
                    artistdata = Useful.getartistdata(song.song_artistId),
                    genredata = Useful.getgenredata(song.song_genre),
                    publisherdata = Useful.getpublishersongdata(song.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)

                };

                o_singhistorydata.Add(new Singhistorydata()
                {
                    singtime = data.singinghistory_date,
                    songdata = songdata
                });
            }

            return new O_SingHistory()
            {
                singhistorydata = o_singhistorydata
            };
        }


        /// <summary>
        /// For get song url and update viewdata (notoken)
        /// </summary>
        /// <param name="i_data">Class I_PlaySong</param>
        /// <returns>Class O_PlaySong</returns>
        [HttpPost]
        [ActionName("Playsong_noToken")]
        public O_PlaySong Playsong_noToken(I_PlaySong i_data)
        {
            if (i_data == null)
            {
                return new O_PlaySong()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            //if (Useful.checklogin(i_data.logindata) != null)
            //{
            //    return new O_PlaySong()
            //    {
            //        errordata = Useful.checklogin(i_data.logindata)
            //    };
            //}
            //int user_id = Useful.getuserid(i_data.logindata.token);
            song datasong = db.song.FirstOrDefault(u => u.song_id == i_data.id);

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, datauser);

            if (datasong == null)
            {
                return new O_PlaySong()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };

            }

            //update song
            //datasong.song_view = datasong.song_view + 1;
            //update artist
            //artist dataartist = db.artist.FirstOrDefault(a => a.artist_id == datasong.song_artistId);
            //dataartist.artist_view = dataartist.artist_view + 1;

            //add singinghistory
            singinghistory singhistory = new singinghistory()
            {
                song_id = i_data.id,
                user_id = 0,
                artist_id = datasong.song_artistId,
                singinghistory_date = DateTime.Now
            };
            db.singinghistory.AddObject(singhistory);

            //viewhistory lastview = db.viewhistory.Where(v => v.Song_Id == i_data.id).OrderByDescending(v => v.ViewHistory_Date).FirstOrDefault();
            //DateTime lastview = view.ViewHistory_Date;

            //if (lastview == null || lastview.ViewHistory_Date.Day != DateTime.Now.Day || lastview.ViewHistory_Date.Month != DateTime.Now.Month)
            //{
            //add viewhistory
            viewhistory viewhistory = new viewhistory()
            {
                Song_Id = i_data.id,
                User_Id = 0,
                ViewHistory_Date = DateTime.Now
            };
            db.viewhistory.AddObject(viewhistory);
            //}

            //add WTBtoken

            //string Token = Useful.GetMd5Hash(MD5.Create(), DateTime.Now.ToString() + i_data.id.ToString() + user_id.ToString() + "song");
            //wtbtokens wtbtokens = new wtbtokens()
            //{
            //    user_id = user_id,
            //    WTBTokens_token = Token,
            //    WTBTokens_ipaddress = HttpContext.Current.Request.UserHostAddress,
            //    //WTBTokens_timestamp = DateTime.Now
            //};
            //wtbtokens lasttoken = db.wtbtokens.Where(w => w.user_id == user_id).FirstOrDefault();
            //if (lasttoken != null)
            //{
            //    lasttoken.user_id = wtbtokens.user_id;
            //    lasttoken.WTBTokens_token = wtbtokens.WTBTokens_token;
            //    lasttoken.WTBTokens_ipaddress = wtbtokens.WTBTokens_ipaddress;
            //}
            //else
            //    db.wtbtokens.AddObject(wtbtokens);
            //save
            db.SaveChanges();


            //Songdata data = new Songdata()
            //{
            //    id = datasong.song_id,
            //    engName = datasong.song_engName,
            //    originName = datasong.song_originName,
            //    lyrics = datasong.song_lyrics,
            //    URL_picture = datasong.song_URL_picture,
            //    price = datasong.song_price,
            //    releasedDate = datasong.song_releasedDate,
            //    //thumbnail = datasong.song_thumbnail,
            //    view = Useful.getview(datasong.song_id),
            //    //filePath = datasong.song_filePath,
            //    length = datasong.song_length,
            //    //keywords = datasong.song_keywords,

            //    //url
            //    //url_iOS = datasong.song_URL_iOS + "?token=" + Token,
            //    //url_Android_Other = datasong.song_URL_Android_Other + "?token=" + Token,
            //    //url_RTMP = datasong.song_URL_RTMP + "?token=" + Token,
            //    url_iOS = datasong.song_URL_iOS,
            //    url_Android_Other = datasong.song_URL_Android_Other,
            //    url_RTMP = datasong.song_URL_RTMP,

            //    //data
            //    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
            //    albumdata = Useful.getalbumdata(datasong.song_albumId),
            //    artistdata = Useful.getartistdata(datasong.song_artistId),
            //    genredata = Useful.getgenredata(datasong.song_genre),
            //    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
            //    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId)

            //};


            return new O_PlaySong()
            {
                songdata = Useful.getsongdata(datasong.song_id,0, "")
            };

        }
        
    }
}