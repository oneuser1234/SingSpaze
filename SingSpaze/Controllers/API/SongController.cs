using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SingSpaze.Models;
using SingSpaze.Models.Input;
using SingSpaze.Models.Output;

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

            List<publisher_song> listsong = new List<publisher_song>();

            var before = DateTime.Now.AddDays(-i_data.time);

            // order
            if (string.IsNullOrEmpty(i_data.type))
                listsong = db.publisher_song.OrderByDescending(s => s.song_id).ToList();
            else if (i_data.type == "new")
                listsong = db.publisher_song.Where(s => s.song_releasedDate > before).OrderByDescending(s => s.song_releasedDate).ToList();
            else if (i_data.type == "hot")
            {
               var grouphistorysong = from history in db.singinghistory
                                      where history.singinghistory_date > before
                                      group history by history.song_id into ghistory
                                      select new { song_id = ghistory.Key, count = ghistory.Count()};

               var joinhistory = from dbsong in db.publisher_song.AsEnumerable()
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

            List<Listsongdata> o_song = new List<Listsongdata>();

            foreach (publisher_song data in listsong)
            {
                o_song.Add(new Listsongdata()
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
                        publisherdata = Useful.getpublisherdata(data.song_publisherId),
                        contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                    });
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
        /// <param name="id">Song_id</param>
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

            publisher_song datasong = db.publisher_song.FirstOrDefault(u => u.song_id == i_data.id);

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
            datasong.song_view = datasong.song_view + 1;

            //add singinghistory
            singinghistory history = new singinghistory()
            {
                song_id = i_data.id,
                user_id = Useful.getuserid(i_data.logindata.token),
                artist_id = datasong.song_artistId,
                singinghistory_date = DateTime.Now
            };
            db.singinghistory.AddObject(history);

            //save
            db.SaveChanges();
            

            Songdata data = new Songdata()
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

                //url
                url_iOS = datasong.song_URL_iOS,
                url_Android_Other = datasong.song_URL_Android_Other,
                url_RTMP = datasong.song_URL_RTMP,

                //data
                languagedata = Useful.getlanguagedata(datasong.song_languageId),
                albumdata = Useful.getalbumdata(datasong.song_albumId),
                artistdata = Useful.getartistdata(datasong.song_artistId),
                genredata = Useful.getgenredata(datasong.song_genre),
                publisherdata = Useful.getpublisherdata(datasong.song_publisherId),
                contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId)
                
            };

            
            return new O_PlaySong()
                {
                  songdata = data  
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

            List<publisher_song> listsong = new List<publisher_song>();

            listsong = (from dbsong in db.publisher_song.ToList()
                       join dbartist in db.publisher_artist.ToList()
                       on dbsong.song_artistId equals dbartist.artist_id
                       join dbalbum in db.album.ToList()
                       on dbsong.song_albumId equals dbalbum.album_id
                       where dbsong.song_status == 1
                       orderby dbsong.song_view descending
                       select dbsong).ToList();
                       


            // where 
            if (i_data.type == "Artist")
                listsong = listsong.Where(s => Useful.getartistdata(s.song_artistId).description_TH.Contains(i_data.keyword)).ToList();
            else if (i_data.type == "Album")
                listsong = listsong.Where(s => Useful.getalbumdata(s.song_albumId).description_TH.Contains(i_data.keyword)).ToList();
            else if (i_data.type == "Lyrics")
                listsong = listsong.Where(s => s.song_lyrics.Contains(i_data.keyword)).ToList();
            else //song name
                listsong = listsong.Where(s => s.song_originName.Contains(i_data.keyword)).ToList();
            
                    

            int resultNumber = listsong.Count();

            // skip take
            listsong = listsong.Skip(i_data.selectdata.startindex-1).Take(i_data.selectdata.endindex-i_data.selectdata.startindex+1).ToList();

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

            List<Listsongdata> o_song = new List<Listsongdata>();

            foreach (publisher_song data in listsong)
            {
                o_song.Add(new Listsongdata()
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
                    publisherdata = Useful.getpublisherdata(data.song_publisherId),
                    contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                });
            }

            return new O_SearchSong()
            {
                listsong = o_song,
                resultNumber = resultNumber
            };
        }


        

        
    }
}