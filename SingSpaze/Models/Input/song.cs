using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SingSpaze.Models.Input
{
    /// <summary>
    /// Class input data for songlist (ex.type,time,ablum_id)
    /// </summary>
    [DataContract]
    public class I_SongList
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }

        /// <summary>
        /// Type of order this list allow "new,hot" (default = null)
        /// </summary>
        [DataMember(Name = "type")]
        public string type { get; set; }
        /// <summary>
        /// Focus in genere_id (optinal)
        /// </summary>
        //[DataMember(Name = "genre_id")]
        //public string genre_id { get; set; }
        /// <summary>
        /// Focus in album_id (optinal)
        /// </summary>
        [DataMember(Name = "album_id")]
        public string album_id { get; set; }
        /// <summary>
        /// Focus in artist_id (optinal)
        /// </summary>
        [DataMember(Name = "artist_id")]
        public string artist_id { get; set; }

        int _language_id = 1;
        /// <summary>
        /// Focus in language_id (default = 1)
        /// </summary>
        [DataMember(Name = "language_id")]
        public int language_id { get { return _language_id; } set { this._language_id = value; } }
         /// <summary>
        /// Focus in categories allow 'pop,rock,luktung,others' (default = null)
        /// </summary>
        [DataMember(Name = "categories")]
        public string categories { get; set; }

        /// <summary>
        /// Countrycode
        /// </summary>
        [DataMember(Name = "countrycode")]
        public string countrycode { get; set; }

        int _time = 30;
        /// <summary>
        /// Number or day to focus (default = 30)
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
    /// Class input data for playsong 
    /// </summary>
    [DataContract]
    public class I_PlaySong
    {
        /// <summary>
        /// Song Id
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; set; }
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
    }

    /// <summary>
    /// Class input data for searchsong (ex.keyword ,type)
    /// </summary>
    [DataContract]
    public class I_SearchSong
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        /// <summary>
        /// Keyword data (default = null)
        /// </summary>
        [DataMember(Name = "keyword")]
        public string keyword { get; set; }
        /// <summary>
        /// Type of search allow "Artist,Album,Lyrics,Song name" (default = Song name)
        /// </summary>
        [DataMember(Name = "type")]
        public string type { get; set; }

        int _language_id = 1;
        /// <summary>
        /// Focus in language_id (default = 1)
        /// </summary>
        [DataMember(Name = "language_id")]
        public int language_id { get { return _language_id; } set { this._language_id = value; } }

        /// <summary>
        /// Countrycode
        /// </summary>
        [DataMember(Name = "countrycode")]
        public string countrycode{ get; set; }

        Selectdata _selectdata = Useful.getbaseselectdata();
        /// <summary>
        /// Class selectdata
        /// </summary>
        [DataMember(Name = "selectdata")]
        public Selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }

    /// <summary>
    /// Class input data for singing history 
    /// </summary>
    [DataContract]
    public class I_SingHistory
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }

        int _time = 30;
        /// <summary>
        /// Number or day to focus (default = 30)
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
    /// Class input data for searchsong v2
    /// </summary>
    [DataContract]
    public class I_SearchSong_v2
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        /// <summary>
        /// Keyword data (default = null)
        /// </summary>
        [DataMember(Name = "keyword")]
        public string keyword { get; set; }
        

        int _language_id = 1;
        /// <summary>
        /// Focus in language_id (default = 1)
        /// </summary>
        [DataMember(Name = "language_id")]
        public int language_id { get { return _language_id; } set { this._language_id = value; } }

        /// <summary>
        /// Countrycode
        /// </summary>
        [DataMember(Name = "countrycode")]
        public string countrycode { get; set; }

        Selectdata _selectdata = Useful.getbaseselectdata();
        /// <summary>
        /// Class selectdata
        /// </summary>
        [DataMember(Name = "selectdata")]
        public Selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    }


    /// <summary>
    /// Class input data for add record 
    /// </summary>
    [DataContract]
    public class I_AddRecord
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        /// <summary>
        /// Song id
        /// </summary>
        [DataMember(Name = "song_id")]
        public long song_id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "description")]
        public string description { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [DataMember(Name = "length")]
        public string length { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [DataMember(Name = "url")]
        public string url { get; set; }

    }

    /// <summary>
    /// Class input data for edit record 
    /// </summary>
    [DataContract]
    public class I_EditRecord
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        /// <summary>
        /// Record id
        /// </summary>
        [DataMember(Name = "record_id")]
        public long record_id { get; set; }

        /// <summary>
        /// Song id
        /// </summary>
        [DataMember(Name = "song_id")]
        public long song_id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "description")]
        public string description { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [DataMember(Name = "length")]
        public string length { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [DataMember(Name = "url")]
        public string url { get; set; }

    }

    /// <summary>
    /// Class input data for delete record 
    /// </summary>
    [DataContract]
    public class I_DeleteRecord
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        /// <summary>
        /// Record id
        /// </summary>
        [DataMember(Name = "record_id")]
        public long record_id { get; set; }
        

    }

    /// <summary>
    /// Class input data for Get Record 
    /// </summary>
    [DataContract]
    public class I_GetRecord
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
        
        Selectdata _selectdata = Useful.getbaseselectdata();
        /// <summary>
        /// Class selectdata
        /// </summary>
        [DataMember(Name = "selectdata")]
        public Selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }

    }

}