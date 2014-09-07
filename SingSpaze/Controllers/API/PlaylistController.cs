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
    /// <summary>
    /// Playlist api
    /// </summary>
    public class PlaylistController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        /// <summary>
        /// Send data to get List playlist
        /// </summary>
        /// <param name="i_data">Class I_ListPlayList</param>
        /// <returns>Class O_ListPlayList</returns>
        [HttpPost]
        [ActionName("ListPlayList")]
        public O_ListPlayList ListPlayList(I_ListPlayList i_data)
        {

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_ListPlayList()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            List<Playlistdata> o_listdata = new List<Playlistdata>();
            List<playlist> listplaylistdata = db.playlist.OrderBy(p => p.playlist_id).Skip(i_data.selectdata.startindex-1).Take(i_data.selectdata.endindex-i_data.selectdata.startindex+1).ToList();

            foreach (playlist data in listplaylistdata)
            {
                Playlistdata playlistdata = new Playlistdata();
                playlistdata.id = data.playlist_id;
                playlistdata.description = data.playlist_description;

                o_listdata.Add(playlistdata);
            }

            return new O_ListPlayList()
            {
                playlists = o_listdata
            };
        }

       
        /// <summary>
        /// Send data to get playlist
        /// </summary>
        /// <param name="i_data">Class I_PlayList</param>
        /// <returns>Class O_PlayList</returns>
        [HttpPost]
        [ActionName("Playlist")]
        public O_PlayList Playlist(I_PlayList i_data) //playlist_id
        {

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_PlayList()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            List<playlisttosong> listplaylisttosong = db.playlisttosong.Where(p => p.playlist_id == i_data.id).OrderBy(p => p.song_id).ToList();

            if (listplaylisttosong == null)
            {
                return new O_PlayList()
                {
                    errordata = new Errordata()
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

            List<Listsongdata> o_listdata = new List<Listsongdata>();

            List<song> listsong = db.song.Where(s => songlist.Contains(s.song_id)).OrderBy(p => p.song_id).Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList();
            foreach (song data in listsong)
            {
                Listsongdata songdata = new Listsongdata()
                {
                    id = data.song_id,
                    originName = data.song_originName,
                    engName = data.song_engName,
                    price = data.song_price,
                    //thumbnail = data.song_thumbnail,
                    URL_picture = data.song_URL_picture,
                    view = Useful.getview(data.song_id),

                    //languagedata = Useful.getlanguagedata(data.song_languageId),
                    albumdata = Useful.getalbumdata(data.song_albumId),
                    artistdata = Useful.getartistdata(data.song_artistId),
                    genredata = Useful.getgenredata(data.song_genre),
                    publisherdata = Useful.getpublisherdata(data.song_publisherId),
                    //contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId),
                    
                };
                o_listdata.Add(songdata);
            }

            return new O_PlayList()
            {
                songlists = o_listdata 
            };

        }


        /// <summary>
        /// Create new list
        /// </summary>
        /// <param name="i_data">Class I_AddList</param>
        /// <returns>Class O_AddList</returns>
        [HttpPost]
        [ActionName("AddList")]
        public O_AddList AddList(I_AddList i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.description))
            {
                return new O_AddList()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_AddList()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }
            
            playlist playlistdata = new playlist()
            {
                 playlist_description = i_data.description,
                 user_id = Useful.getuserid(i_data.logindata.token),
                 playlist_createdDatetime = DateTime.Now

            };

            db.playlist.AddObject(playlistdata);
            db.SaveChanges();

            return new O_AddList()
            {
                result = true
            };

        }

        /// <summary>
        /// Add song to playlist
        /// </summary>
        /// <param name="i_data">Class I_AddSong</param>
        /// <returns>Class O_AddSong</returns>
        [HttpPost]
        [ActionName("AddSong")]
        public O_AddSong AddSong(I_AddSong i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.song_id.ToString()) || string.IsNullOrEmpty(i_data.playlist_id.ToString()))
            {
                return new O_AddSong()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_AddSong()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            playlisttosong checkdata = db.playlisttosong.Where(p => p.playlist_id == i_data.playlist_id && p.song_id == i_data.song_id).SingleOrDefault();

            if (checkdata != null)
            {
                return new O_AddSong()
                {
                    result = false,
                    errordata = new Errordata()
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