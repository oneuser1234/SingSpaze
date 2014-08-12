using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SingSpaze.Models.Input
{
    /// <summary>
    /// Class input data for artistlist (ex.type,artist_id)
    /// </summary>
    [DataContract]
    public class I_ArtistList
    {
        /// <summary>
        /// Data for order allow "hot" (optional)
        /// </summary>
        [DataMember(Name = "type")]
        public string type { get; set; }
        /// <summary>
        /// Focus artist_id
        /// </summary>
        [DataMember(Name = "artist_id")]
        public string artist_id { get; set; }
        
        int _time = 30;
        /// <summary>
        /// This is ago days to calulation for hot artisit (if type was null it's will be ignore) (defult was 30)
        /// </summary>
        [DataMember(Name = "time")]
        public int time { get { return _time; } set { this._time = value; } }

        Selectdata _selectdata = Useful.getbaseselectdata();
        /// <summary>
        /// Class selectdata
        /// </summary>
        [DataMember(Name = "selectdata")]
        public Selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }
}