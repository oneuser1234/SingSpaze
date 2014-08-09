using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Input
{
    public class I_ArtistList
    {
        public string type { get; set; }
        public string artist_id { get; set; }
        
        int _time = 30;
        public int time { get { return _time; } set { this._time = value; } }
        selectdata _selectdata = Useful.getbaseselectdata();
        public selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }
}