using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingSpaze.Models.Management
{
    public class Song
    {
        public int Id { get; set; }
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
        
        public int Id { get; set; }
        public string Name_TH { get; set; }
        public string Name_EN { get; set; }
        public int Artist { get; set; }
        public int Album { get; set; }
        public int Language { get; set; }
        public string ReleasedDate { get; set; }
        public string Length { get; set; }
        public int Genre { get; set; }
        public int Publisher { get; set; }
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