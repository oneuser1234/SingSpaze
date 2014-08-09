using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    public class O_ListPlayList
    {
        public IEnumerable<playlistdata> playlists { get; set; }
        public errordata errordata { get; set; }
    }

    public class O_PlayList
    {
        public IEnumerable<songdata> playlists { get; set; }
        public errordata errordata { get; set; }
    }

    public class O_AddList
    {
        public Boolean result { get; set; }
        public errordata errordata { get; set; }

    }

    public class O_AddSong
    {
        public Boolean result { get; set; }
        public errordata errordata { get; set; }

    }
    
    


}