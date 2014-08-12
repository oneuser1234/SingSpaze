using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output for songlist (ex.resultNumber ,listsong)
    /// </summary>
    public class O_SongList
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class Listsongdata
        /// </summary>
        public List<Listsongdata> listsong { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for playsong  (ex.Songdata ,Errordata)
    /// </summary>
    public class O_PlaySong
    {
        /// <summary>
        /// Class songdata
        /// </summary>
        public Songdata songdata { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for searchsong (ex.resultNumber ,listsong)
    /// </summary>
    public class O_SearchSong
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class listsongdata
        /// </summary>
        public List<Listsongdata> listsong { get; set; }
        
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

}