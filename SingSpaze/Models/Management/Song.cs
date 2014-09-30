using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingSpaze.Models.Management
{
    public class Song
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Length { get; set; }
        public int Status { get; set; }
    }

    public class Editsong
    {
        
        public long Id { get; set; }
        public string Name_TH { get; set; }
        public string Name_EN { get; set; }
        public long Artist { get; set; }
        public long Album { get; set; }
        public long Language { get; set; }
        public string ReleasedDate { get; set; }
        public string Length { get; set; }
        public long Genre { get; set; }
        public long Publisher { get; set; }
        public string Lyrics { get; set; }
        public int Status { get; set; }

        public DateTime AddDate { get; set; }

        public SelectList ArtistList { get; set; }
        public SelectList AlbumList { get; set; }
        public SelectList LanguageList { get; set; }
        public SelectList GenreList { get; set; }
        public SelectList ContentPartnerList { get; set; }
        public SelectList PublisherList { get; set; }
        
        
    }
}