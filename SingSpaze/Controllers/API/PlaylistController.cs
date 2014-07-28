using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SingSpaze.Models;
using SingSpaze.Models.Output;
using SingSpaze.Models.Input;

namespace SingSpaze.Controllers.API
{
    public class PlaylistController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        //[Authorize]
        [HttpPost]
        [ActionName("List")]
        public O_ListPlayList ListPlayList(I_ListPlayList i_data)
        {
           
            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_ListPlayList()
                {
                    errordata = new errordata()
                    {
                        code = 5,
                        Detail = Useful.geterrordata(5)
                    }
                };
            }

            List<playlistdata> o_listdata = new List<playlistdata>();
            List<playlist> listplaylistdata = db.playlist.OrderBy(p => p.playlist_id).Skip(i_data.selectdata.skip).Take(i_data.selectdata.take).ToList();

            foreach (playlist data in listplaylistdata)
            {
                playlistdata playlistdata = new playlistdata();
                playlistdata.id = data.playlist_id;
                playlistdata.description = data.playlist_description;

                o_listdata.Add(playlistdata);
            }

            return new O_ListPlayList()
            {
                playlists = o_listdata
            };
        }

       

        //[Authorize]
        [HttpPost]
        [ActionName("Data")]
        public O_PlayList playlist(int id,I_PlayList i_data) //playlist_id
        {
            
            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_PlayList()
                {
                    errordata = new errordata()
                    {
                        code = 5,
                        Detail = Useful.geterrordata(5)
                    }
                };
            }

            List<playlisttosong> listplaylisttosong = db.playlisttosong.Where(p => p.playlist_id == id).OrderBy(p => p.song_id).Skip(i_data.selectdata.skip).Take(i_data.selectdata.take).ToList();

            if (listplaylisttosong == null)
            {
                return new O_PlayList()
                {
                    errordata = new errordata()
                    {
                        code = 10,
                        Detail = Useful.geterrordata(10)
                    }
                };
            }


            List<int> songlist = new List<int>();
            foreach (playlisttosong data in listplaylisttosong)
            {
                songlist.Add(data.song_id);
            }

            List<songdata> o_listdata = new List<songdata>();

            List<song> listsong = db.song.Where(s => songlist.Contains(s.song_id)).ToList();
            foreach (song data in listsong)
            {
                songdata songdata = new songdata()
                {
                    id = data.song_id,
                    engName = data.song_engName,
                    originName = data.song_originName,                    
                    lyrics = data.song_lyrics,
                    picture = data.song_picture,
                    price = data.song_price,
                    releasedDate = data.song_releasedDate,
                    thumbnail = data.song_thumbnail,
                    view = data.song_view,

                    languagedata = Useful.getlanguagedata(data.song_languageId),
                    albumdata = Useful.getalbumdata(data.song_albumId),
                    artistdata = Useful.getartistdata(data.song_artistId),
                    genredata = Useful.getgenredata(data.song_genre),
                    recordlabeldata = Useful.getrecordlabeldata(data.song_recordLabelId),
                    contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId)
                };
                o_listdata.Add(songdata);
            }

            return new O_PlayList()
            {
                playlists = o_listdata 
            };

        }



        [HttpPost]
        [ActionName("AddList")]
        public O_AddList AddList(I_AddList i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.description))
            {
                return new O_AddList()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_AddList()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 5,
                        Detail = Useful.geterrordata(5)
                    }
                };
            }

            playlist playlistdata = new playlist()
            {
                 playlist_description = i_data.description,
                 user_id = i_data.logindata.id,
                 playlist_createdDatetime = DateTime.Now

            };

            db.playlist.AddObject(playlistdata);
            db.SaveChanges();

            return new O_AddList()
            {
                result = true
            };

        }


        [HttpPost]
        [ActionName("AddSong")]
        public O_AddSong AddSong(I_AddSong i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.song_id.ToString()) || string.IsNullOrEmpty(i_data.playlist_id.ToString()))
            {
                return new O_AddSong()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_AddSong()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 5,
                        Detail = Useful.geterrordata(5)
                    }
                };
            }

            playlisttosong checkdata = db.playlisttosong.Where(p => p.playlist_id == i_data.playlist_id && p.song_id == i_data.song_id).SingleOrDefault();

            if (checkdata != null)
            {
                return new O_AddSong()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 12,
                        Detail = Useful.geterrordata(12)
                    }
                };
            }


            playlisttosong playlisttosongdata = new playlisttosong()
            {
                playlist_id = i_data.playlist_id,
                song_id = i_data.song_id

            };

            db.playlisttosong.AddObject(playlisttosongdata);
            db.SaveChanges();

            return new O_AddSong()
            {
                result = true
            };

        }


    }
}