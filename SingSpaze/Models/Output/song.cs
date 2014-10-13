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
        public List<Songdata> listsong { get; set; }
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
        public List<Songdata> listsong { get; set; }
        
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }


    /// <summary>
    /// Class output for sing history (ex.listsinghistory)
    /// </summary>
    public class O_SingHistory
    {
        /// <summary>
        /// List class singhistorydata
        /// </summary>
        public List<Singhistorydata> singhistorydata { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for add record
    /// </summary>
    public class O_AddRecord
    {
        /// <summary>
        /// result true or false
        /// </summary>
        public Boolean result { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for edit record
    /// </summary>
    public class O_EditRecord
    {
        /// <summary>
        /// result true or false
        /// </summary>
        public Boolean result { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for delete record
    /// </summary>
    public class O_DeleteRecord
    {
        /// <summary>
        /// result true or false
        /// </summary>
        public Boolean result { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for Get Record (ex.listrecord)
    /// </summary>
    public class O_GetRecord
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultnumber { get; set; }
        /// <summary>
        /// List class singhistorydata
        /// </summary>
        public List<Recorddata> recorddata { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

}