using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Input
{
    public class I_ListPlayList
    {
        public logindata logindata { get; set; }
        selectdata _selectdata = Useful.getbaseselectdata();
        public selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }

    public class I_PlayList
    {
        public logindata logindata { get; set; }
        selectdata _selectdata = Useful.getbaseselectdata();
        public selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }

    public class I_AddList
    {
        public string description { get; set; }

        public logindata logindata { get; set; }
    }

    public class I_AddSong
    {
        public int song_id { get; set; }
        public int playlist_id { get; set; }

        public logindata logindata { get; set; }
    }
}