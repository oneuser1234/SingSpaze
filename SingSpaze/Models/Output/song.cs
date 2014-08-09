using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    public class O_SongList
    {
        public IEnumerable<listsongdata> listsong { get; set; }
        public errordata errordata { get; set; }
    }

    //public class O_Song
    //{
        
    //    public errordata errordata { get; set; }
    //}

    public class O_PlaySong
    {
        public songdata songdata { get; set; }        
        public errordata errordata { get; set; }
    }

}