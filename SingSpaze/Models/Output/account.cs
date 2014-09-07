using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output data for register
    /// </summary>
    public class O_Register
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

    public class O_FBRegister
    {
        public Boolean result { get; set; }
        public Errordata errordata { get; set; }
    }
    
    /// <summary>
    /// Class output data for login
    /// </summary>
    public class O_Login
    {
        /// <summary>
        /// Unique string for connection
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output data for FBlogin
    /// </summary>
    public class O_FBLogin
    {
       
        /// <summary>
        /// Unique string for connection
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output data for profile
    /// </summary>
    public class O_Profile
    {
        /// <summary>
        /// Class userdata
        /// </summary>
        public Userdata userdata { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for editprofile
    /// </summary>
    public class O_EditProfile
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
    /// Class output data for forgot
    /// </summary>
    public class O_Forgot
    {
        /// <summary>
        /// Reset Token
        /// </summary>
        public string retoken { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
    /// <summary>
    /// Class output data for reset
    /// </summary>
    public class O_Reset
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
    /// Class output data for upload picture
    /// </summary>
    public class O_Upload
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