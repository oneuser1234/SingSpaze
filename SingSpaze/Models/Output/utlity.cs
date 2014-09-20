using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output data for request song
    /// </summary>
    public class O_SongRequest
    {
        /// <summary>
        /// Return True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    
    /// <summary>
    /// Class output data for contactus
    /// </summary>
    public class O_ContactUs
    {
        /// <summary>
        /// Return True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
}