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
        [ActionName("GetPlaylistList")]
        public O_GetPlaylistList GetPlaylistList (I_GetPlaylistList i_data)
        {

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_GetPlaylistList()
                {                    
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            long user_id = Useful.getuserid(i_data.logindata.token);
            List<Playlistdata> o_listdata = new List<Playlistdata>();
            List<playlist> listplaylistdata = db.playlist.Where(p => p.user_id == user_id).OrderBy(p => p.playlist_id).ToList();
            int resultNumber = listplaylistdata.Count;
            //skip
            listplaylistdata = listplaylistdata.Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList();

            foreach (playlist data in listplaylistdata)
            {
                Playlistdata playlistdata = new Playlistdata();
                playlistdata.id = data.playlist_id;
                playlistdata.description = data.playlist_description;

                o_listdata.Add(playlistdata);
            }

            return new O_GetPlaylistList()
            {
                resultNumber = resultNumber,
                playlists = o_listdata
            };
        }

       
        /// <summary>
        /// Send data to get playlist
        /// </summary>
        /// <param name="i_data">Class I_PlayList</param>
        /// <returns>Class O_PlayList</returns>
        [HttpPost]
        [ActionName("GetSonginPlaylist")]
        public O_GetSonginPlaylist GetSonginPlaylist(I_GetSonginPlaylist i_data) //playlist_id
        {

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_GetSonginPlaylist()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            List<playlisttosong> listplaylisttosong = db.playlisttosong.Where(p => p.playlist_id == i_data.id).OrderBy(p => p.song_id).ToList();
            int resultNumber = listplaylisttosong.Count;
            if (listplaylisttosong == null)
            {
                return new O_GetSonginPlaylist()
                {
                    errordata = Useful.geterror(6)
                };
            }


            List<long> songlist = new List<long>();
            foreach (playlisttosong data in listplaylisttosong)
            {
                songlist.Add(data.song_id);
            }

            List<Playlistsongdata> o_listdata = new List<Playlistsongdata>();

            List<song> listsong = db.song.Where(s => songlist.Contains(s.song_id)).OrderBy(p => p.song_id).ToList();
            foreach (song data in listsong)
            {
                Songdata songdata = new Songdata()
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
                    publisherdata = Useful.getpublishersongdata(data.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(data.song_contentPartnerId),
                    
                };

                Playlistsongdata listsongdata = new Playlistsongdata()
                {
                    songdata = songdata,
                    sequence = listplaylisttosong.Where(l => l.song_id == data.song_id).Select(l => l.playlistToSong_sequence).FirstOrDefault()
                };
                o_listdata.Add(listsongdata);
            }

            return new O_GetSonginPlaylist()
            {
                resultNumber = resultNumber,
                songlists = o_listdata.OrderBy(l => l.sequence).Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList() 
            };

        }


        /// <summary>
        /// Add new playlist
        /// </summary>
        /// <param name="i_data">Class I_AddNewPlaylist</param>
        /// <returns>Class O_AddNewPlaylist</returns>
        [HttpPost]
        [ActionName("AddNewPlaylist")]
        public O_AddNewPlaylist AddNewPlaylist(I_AddNewPlaylist i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.description))
            {
                return new O_AddNewPlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_AddNewPlaylist()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }
            
            playlist playlistdata = new playlist()
            {
                 playlist_description = i_data.description,
                 user_id = Useful.getuserid(i_data.logindata.token),
                 playlist_createdDatetime = DateTime.Now

            };

            db.playlist.Add(playlistdata);
            db.SaveChanges();

            return new O_AddNewPlaylist()
            {
                result = true
            };

        }

        /// <summary>
        /// Remove playlist
        /// </summary>
        /// <param name="i_data">Class I_RemovePlaylist</param>
        /// <returns>Class O_RemovePlaylist</returns>
        [HttpPost]
        [ActionName("RemovePlaylist")]
        public O_RemovePlaylist RemovePlaylist(I_RemovePlaylist i_data)
        {
            if (i_data == null)
            {
                return new O_RemovePlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_RemovePlaylist()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }
            long user_id = Useful.getuserid(i_data.logindata.token);
            playlist playlistdata = db.playlist.SingleOrDefault(p => p.playlist_id == i_data.id && p.user_id == user_id);

            if (playlistdata != null)
            {
                List<playlisttosong> playlisttosongdata = db.playlisttosong.Where(p => p.playlist_id == i_data.id).ToList();
                foreach (playlisttosong data in playlisttosongdata)
                {
                    db.playlisttosong.Remove(data);
                }
                db.playlist.Remove(playlistdata);
                db.SaveChanges();
            }
            else
            {
                return new O_RemovePlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(10)
                };
            }

            return new O_RemovePlaylist()
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
        [ActionName("AddSongtoPlaylist")]
        public O_AddSongtoPlaylist AddSongtoPlaylist(I_AddSongtoPlaylist i_data)
        {
            if (i_data == null || String.IsNullOrEmpty(i_data.song_id.ToString()) || string.IsNullOrEmpty(i_data.playlist_id.ToString()))
            {
                return new O_AddSongtoPlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_AddSongtoPlaylist()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }
            
            long user_id = Useful.getuserid(i_data.logindata.token);
            playlisttosong checkdata = db.playlisttosong.Where(p => p.playlist_id == i_data.playlist_id && p.song_id == i_data.song_id).SingleOrDefault();

            if (checkdata != null)
            {
                return new O_AddSongtoPlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(12)
                };
            }

            playlist checkplaylist = db.playlist.SingleOrDefault(p => p.user_id == user_id && p.playlist_id == i_data.playlist_id);
            
            if (checkplaylist != null)
            {
                int sequence = db.playlisttosong.Where(p => p.playlist_id == i_data.playlist_id).Select(p => p.playlistToSong_sequence).DefaultIfEmpty().Max();
                               
               
                playlisttosong playlisttosongdata = new playlisttosong()
                {
                    playlist_id = i_data.playlist_id,
                    song_id = i_data.song_id,
                    playlistToSong_sequence = sequence + 1

                };

                db.playlisttosong.Add(playlisttosongdata);
                db.SaveChanges();
            }
            else
            {
                return new O_AddSongtoPlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(10)
                };
            }

            return new O_AddSongtoPlaylist()
            {
                result = true
            };

        }

        /// <summary>
        /// Update playlist
        /// </summary>
        /// <param name="i_data">Class I_UpdatePlaylist</param>
        /// <returns>Class O_UpdatePlaylist</returns>
        [HttpPost]
        [ActionName("UpdatePlaylist")]
        public O_UpdatePlaylist UpdatePlaylist(I_UpdatePlaylist i_data)
        {
            if (i_data == null || string.IsNullOrEmpty(i_data.playlist_id.ToString()))
            {
                return new O_UpdatePlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_UpdatePlaylist()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            long user_id = Useful.getuserid(i_data.logindata.token);
            

            playlist checkplaylist = db.playlist.SingleOrDefault(p => p.user_id == user_id && p.playlist_id == i_data.playlist_id);

            if (checkplaylist != null)
            {
                //change name
                if(!string.IsNullOrEmpty(i_data.playlist_name))
                    checkplaylist.playlist_description = i_data.playlist_name;
                //remove old playlisttosong
                List<playlisttosong> oldplaylisttosongdata = db.playlisttosong.Where(p => p.playlist_id == i_data.playlist_id).ToList();
                foreach (playlisttosong data in oldplaylisttosongdata)
                {
                    db.playlisttosong.Remove(data);
                }

                List<long> listsong = Useful.getlistdata(i_data.song_id);
                List<long> insertsong = new List<long>();
               
                int sequence = 0;
                foreach (long data in listsong)
                {
                    if(!insertsong.Contains(data))
                    {

                        sequence = sequence + 1;
                        playlisttosong playlisttosongdata = new playlisttosong()
                        {
                            playlist_id = i_data.playlist_id,
                            song_id = Int32.Parse(data.ToString()),
                            playlistToSong_sequence = sequence

                        };

                        db.playlisttosong.Add(playlisttosongdata);
                        insertsong.Add(data);
                    }
                }
                db.SaveChanges();
            }
            else
            {
                return new O_UpdatePlaylist()
                {
                    result = false,
                    errordata = Useful.geterror(10)
                };
            }

            return new O_UpdatePlaylist()
            {
                result = true
            };

        }
    }
}