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
    
    public partial class playlisttosong
    {
        public long playlisttosong_id { get; set; }
        public long playlist_id { get; set; }
        public long song_id { get; set; }
        public int playlistToSong_sequence { get; set; }
    
        public virtual playlist playlist { get; set; }
        public virtual song song { get; set; }
    }
}
