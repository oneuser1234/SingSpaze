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
    
    public partial class wtbtokens
    {
        public long WTBTokens_id { get; set; }
        public long user_id { get; set; }
        public System.DateTime WTBTokens_timestamp { get; set; }
        public string WTBTokens_ipaddress { get; set; }
        public string WTBTokens_token { get; set; }
    
        public virtual user user { get; set; }
    }
}
