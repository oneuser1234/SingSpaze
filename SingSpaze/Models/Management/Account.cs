using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Management
{
    public class Login
    {
        public string user_login { get; set; }
        public string user_password { get; set; }
    }

    public class Editaccount
    {
        public long id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string facebookid { get; set; }
    }
}