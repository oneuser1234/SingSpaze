using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// class O_Register
    /// </summary>
    public class O_Register
    {
        public Boolean result { get; set; }
        public errordata errordata { get; set; }
    }
    
    public class O_Login
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public errordata errordata { get; set; }
    }

    //public class O_ListProfile
    //{
    //    public IEnumerable<userdata> userdata { get; set; }
    //    public errordata errordata { get; set; }
    //}

    public class O_Profile
    {
        public userdata userdata { get; set; }
        public errordata errordata { get; set; }
    }

    public class O_EditProfile
    {
        public Boolean result { get; set; }
        public errordata errordata { get; set; }
    }

    public class O_Forgot
    {
        public int id { get; set; }
        public string retoken { get; set; }
        public errordata errordata { get; set; }
    }

    public class O_Reset
    {
        public Boolean result { get; set; }
        public errordata errordata { get; set; }
    }

}