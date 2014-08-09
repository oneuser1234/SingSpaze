using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SingSpaze.Models.Parameter
{
    public class I_Register
    {
        
        public string Username { get; set; }
        public string Password { get; set; }       
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        int _fbUserId = 0;
        public int fbUserId { get { return _fbUserId; } set { this._fbUserId = value; } }
    }

    public class I_Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        int _fbUserId = 0;
        public int fbUserId { get { return _fbUserId; } set { this._fbUserId = value; } }
    }

    //public class I_ListProfile
    //{
    //    public logindata logindata { get; set; }
    //    selectdata _selectdata = Useful.getbaseselectdata();
    //    public selectdata selectdata { get { return _selectdata; } set { this._selectdata = value; } }
    //}

    public class I_Profile
    {
        public logindata logindata { get; set; }        
    }    

    public class I_EditProfile
    {
        
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public logindata logindata { get; set; }
    }

    public class I_Forgot 
    {
        public string email { get; set; }
    }

    public class I_Reset
    {
        public int id { get; set; }
        public string retoken { get; set; }
        public string newpassword { get; set; }
    }

}