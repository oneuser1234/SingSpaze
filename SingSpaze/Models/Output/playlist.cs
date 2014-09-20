using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output data for list playlist
    /// </summary>
    public class O_GetPlaylistList
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class playlistdata
        /// </summary>
        public List<Playlistdata> playlists { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for song in playlist
    /// </summary>
    public class O_GetSonginPlaylist
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class Listsongdata
        /// </summary>
        public List<Playlistsongdata> songlists { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for AddNewPlaylist
    /// </summary>
    public class O_AddNewPlaylist
    {
        /// <summary>
        /// Return True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }

    }
    /// <summary>
    /// Class output data for ARemovePlaylist
    /// </summary>
    public class O_RemovePlaylist
    {
        /// <summary>
        /// Return True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }

    }
    /// <summary>
    /// Class output data for addsong to playlist
    /// </summary>
    public class O_AddSongtoPlaylist
    {
        /// <summary>
        /// Reture True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }

    }
    /// <summary>
    /// Class output data for update playlist
    /// </summary>
    public class O_UpdatePlaylist
    {
        /// <summary>
        /// Reture True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }

    }
    


}