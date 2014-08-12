using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output data for list playlist
    /// </summary>
    public class O_ListPlayList
    {
        /// <summary>
        /// List class playlistdata
        /// </summary>
        public List<playlistdata> playlists { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for playlist
    /// </summary>
    public class O_PlayList
    {
        /// <summary>
        /// List class songdata
        /// </summary>
        public List<Songdata> playlists { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for addlist
    /// </summary>
    public class O_AddList
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
    public class O_AddSong
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