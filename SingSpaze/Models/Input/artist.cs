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
        /// Data for order allow "hot" (Default = Name)
        /// </summary>
        [DataMember(Name = "type")]
        public string type { get; set; }
        /// <summary>
        /// Focus artist_id (Optional)
        /// </summary>
        [DataMember(Name = "artist_id")]
        public string artist_id { get; set; }
        /// <summary>
        /// Focus artist type allow 'male','female','band' (Optional)
        /// </summary>
        [DataMember(Name = "artist_type")]
        public string artist_type { get; set; }

        int _time = 30;
        /// <summary>
        /// This is ago days to calulation for hot artisit (if type was null it's will be ignore) (Default = 30)
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

    /// <summary>
    /// Class input data for artistdetails (artist Id)
    /// </summary>
    [DataContract]
    public class I_ArtistDetails
    {
        /// <summary>
        /// Artist Id
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; set; }
        
       
    }

    /// <summary>
    /// Class input data for search Artis (ex.keyword ,type)
    /// </summary>
    [DataContract]
    public class I_SearchArtist
    {
        /// <summary>
        /// Keyword data (default = null)
        /// </summary>
        [DataMember(Name = "keyword")]
        public string keyword { get; set; }
        /// <summary>
        /// Type of search allow "male,female,band" (Optional)
        /// </summary>
        [DataMember(Name = "type")]
        public string type { get; set; }        


        Selectdata _selectdata = Useful.getbaseselectdata();
        /// <summary>
        /// Class selectdata
        /// </summary>
        [DataMember(Name = "selectdata")]
        public Selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }
}