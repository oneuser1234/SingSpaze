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


    /// <summary>
    /// Class output getSplashPage
    /// </summary>
    public class O_SplashPage
    {
        
        /// <summary>
        /// List Class SplashPagedata
        /// </summary>
        public List<SplashPagedata> splashpagedata { get; set; }
    }

    /// <summary>
    /// Class output Push_Notification
    /// </summary>
    public class O_Push_Notification
    {

        /// <summary>
        /// Return True or False
        /// </summary>
        public Boolean result { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string errormessage { get; set; }
    }
}