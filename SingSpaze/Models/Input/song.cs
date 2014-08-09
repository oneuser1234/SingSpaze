using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Input
{
    public class I_SongList
    {
        public string type { get; set; }        
        public string genre_id { get; set; }
        public string album_id { get; set; }
        public string artist_id { get; set; }
        public string language_id { get; set; }

        int _time = 30;
        public int time { get { return _time; } set { this._time = value; } }
        selectdata _selectdata = Useful.getbaseselectdata(); 
        public selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }

    //public class I_Song
    //{
    //    public logindata logindata { get; set; }
    //}

    public class I_PlaySong
    {
        public logindata logindata { get; set; }
    }
}