//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SingSpaze.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class songrequest
    {
        public int songRequest_id { get; set; }
        public int user_id { get; set; }
        public System.DateTime songRequest_datetime { get; set; }
        public string songRequest_songname { get; set; }
        public string songRequest_artistname { get; set; }
    }
}
