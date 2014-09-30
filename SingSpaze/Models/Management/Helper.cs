using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Configuration;

namespace SingSpaze.Models.Management
{
    public class CSVList
    {
        //public CSVList(string firstname, string lastname, DateTime dob, string email)
        //{
        //    this.FirstName = firstname;
        //    this.LastName = lastname;
        //    this.Dob = dob;
        //    this.Email = email;
        //}

        public long id { set; get; }
        public string originname { set; get; }
        public string engname { set; get; }
        public string photo { set; get; }
        public string artistTH { set; get; }
        public string artistEN { set; get; }
        public string length { set; get; }
        public string lyrics { set; get; }
        public string albumTH { set; get; }
        public string albumEN { set; get; }
        public DateTime released { set; get; }
        public string views { set; get; }
        public string language { set; get; }
        public string genres { set; get; }
        public string publisher { set; get; }
        public int status { set; get; }
        public decimal? price { set; get; }
        public string copyright { set; get; }
        public int? tracknumber { set; get; }
        public string url_iOS { set; get; }
        public string url_Android { set; get; }
        public string url_RTMP { set; get; }

    }
        

        
    
}