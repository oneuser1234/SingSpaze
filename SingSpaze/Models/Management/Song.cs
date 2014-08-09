using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingSpaze.Models.Management
{
    public class editsong
    {
        
        public int Id { get; set; }
        public string Name_TH { get; set; }
        public string Name_EN { get; set; }
        public int Artist { get; set; }
        public int Album { get; set; }
        public int Language { get; set; }
        public string ReleasedDate { get; set; }
        public decimal Length { get; set; }
        public int Genre { get; set; }
        public int RecordLabel { get; set; }
        public string Lyrics { get; set; }
        public int Status { get; set; }

        public DateTime AddDate { get; set; }

        public SelectList ArtistList { get; set; }
        public SelectList AlbumList { get; set; }
        public SelectList LanguageList { get; set; }
        public SelectList GenreList { get; set; }
        public SelectList ContentPartnerList { get; set; }
        public SelectList RecordLabelList { get; set; }
        
        
    }
}