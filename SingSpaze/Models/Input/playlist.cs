using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SingSpaze.Models.Input
{
    /// <summary>
    /// Class input data for list platlist (ex.logindata,selectdata)
    /// </summary>
    [DataContract]
    public class I_ListPlayList
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
    /// <summary>
    /// Class input data for playlist (ex.logindata,selectdata)
    /// </summary>
    [DataContract]
    public class I_PlayList
    {
        /// <summary>
        /// Playlist id
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; set; }
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
    /// <summary>
    /// Class input data for addlist
    /// </summary>
    [DataContract]
    public class I_AddList
    {
        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "description")]
        public string description { get; set; }
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
    }
    /// <summary>
    /// Class input to addsong to playlist
    /// </summary>
    [DataContract]
    public class I_AddSong
    {
        /// <summary>
        /// Song Id
        /// </summary>
        [DataMember(Name = "song_id")]
        public int song_id { get; set; }
        /// <summary>
        /// Playlist Id
        /// </summary>
        [DataMember(Name = "playlist_id")]
        public int playlist_id { get; set; }
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
    }
}