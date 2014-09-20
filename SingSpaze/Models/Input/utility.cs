using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SingSpaze.Models.Input
{
        /// <summary>
        /// Class input data for song request 
        /// </summary>
        [DataContract]
        public class I_SongRequest
        {
            /// <summary>
            /// Song name
            /// </summary>
            [DataMember(Name = "songname")]
            public string songname { get; set; }
            /// <summary>
            /// Artist name
            /// </summary>
            [DataMember(Name = "artistname")]
            public string artistname { get; set; }
            /// <summary>
            /// Class logindata
            /// </summary>
            [DataMember(Name = "logindata")]
            public Logindata logindata { get; set; }
        }

        /// <summary>
        /// Class input data for contact us 
        /// </summary>
        [DataContract]
        public class I_ContactUs
        {
            /// <summary>
            /// Name
            /// </summary>
            [DataMember(Name = "name")]
            public string name { get; set; }
            /// <summary>
            /// Email
            /// </summary>
            [DataMember(Name = "email")]
            public string email { get; set; }
            /// <summary>
            /// Phone
            /// </summary>
            [DataMember(Name = "phone")]
            public string phone { get; set; }
            /// <summary>
            /// Message
            /// </summary>
            [DataMember(Name = "message")]
            public string message { get; set; }
        }
    
}