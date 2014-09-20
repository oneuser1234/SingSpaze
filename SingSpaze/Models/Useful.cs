using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using SingSpaze.Models.Input;
using System.Runtime.Serialization;

namespace SingSpaze.Models
{
    /// <summary>
    /// Class Useful
    /// </summary>
    public static class Useful
    {
        public static string geterrordata(int code_id)
        {
            string response = "";
            if (code_id == 1)
                response = "cannot register";
            else if (code_id == 2)
                response = "cannot use this username or email";
            else if (code_id == 3)
                response = "username or password was empty";
            else if (code_id == 4)
                response = "username or password was wrong";
            else if (code_id == 5)
                response = "please login before";
            else if (code_id == 6)
                response = "no data";
            else if (code_id == 7)
                response = "your old password was wrong";
            else if (code_id == 8)
                response = "no data to update";
            else if (code_id == 9)
                response = "cannot edit(input wrong)";
            else if (code_id == 10)
                response = "no permisstion to do this";
            else if (code_id == 11)
                response = "please insert data";
            else if (code_id == 12)
                response = "this playlist have this song";
            else if (code_id == 13)
                response = "facebook account can not do this";
            else if (code_id == 14)
                response = "input data was wrong";
            else if (code_id == 15)
                response = "your account was multiple login";

            return response;

        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Errordata checklogin(Logindata logindata)
        {
            if (logindata == null || string.IsNullOrEmpty(logindata.token))
                return new Errordata()
                           {
                               code = 5,
                               Detail = Useful.geterrordata(5)
                           };

            if (getuserid(logindata.token) != 0)
                return null;
            else
                return new Errordata()
                {
                    code = 15,
                    Detail = Useful.geterrordata(15)
                }; ;

        }

        public static Selectdata getbaseselectdata()
        {
            return new Selectdata()
            {
                startindex = 1,
                endindex = 15
            };
        }

        public static List<long> getlistdata(string data)
        {
            List<long> response = new List<long>();
            string[] arraydata = data.Split(',');

            foreach (string stringdata in arraydata)
                response.Add(Int32.Parse(stringdata));

            return response;
        }

        public static Albumdata getalbumdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            album data = db.album.Where(a => a.album_id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Albumdata()
            {
                id = id,
                description_TH = data.album_description_th,
                description_EN = data.album_description_en
            };
                       
        }

        public static Artistdata getartistdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            artist data = db.artist.Where(a => a.artist_id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Artistdata()
            {
                id = id,
                description_TH = data.artist_description_th,
                description_EN = data.artist_description_en,
                picture = data.artist_picture,
                artistType = data.artist_type,
                songs = db.song.Where(s => s.song_artistId == data.artist_id).Count(),
                publisherdata = Useful.getpublisherartistdata(data.artist_publisherforartistId)
            };

        }

        public static Genredata getgenredata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            genre data = db.genre.Where(a => a.genre_id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Genredata()
            {
                id = id,
                description = data.genre_description
            };

        }

        public static Publisherdata getpublishersongdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            publisherforsong data = db.publisherforsong.Where(a => a.publisherforsong_Id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Publisherdata()
            {
                id = id,
                description = data.publisherforsong_description
            };

        }

        public static Publisherdata getpublisherartistdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            publisherforartist data = db.publisherforartist.Where(a => a.publisherforartist_Id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Publisherdata()
            {
                id = id,
                description = data.publisherforartist_description
            };

        }

        //public static Contentpartnerdata getcontentpartnerdata(int id)
        //{
        //    singspazeEntities db = new singspazeEntities();
        //    contentpartner data = db.contentpartner.Where(a => a.contentpartner_id == id).SingleOrDefault();
        //    if (data == null)
        //        return null;
        //    return new Contentpartnerdata()
        //    {
        //        id = id,
        //        description = data.contentpartner_description
        //    };
        //
        //}

        public static Languagedata getlanguagedata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            language data = db.language.Where(a => a.language_id == id).SingleOrDefault();
            if (data == null)
                return null;
            return new Languagedata()
            {
                id = id,
                description = data.language_description
            };

        }

        public static int getuserid(string token)
        {
            singspazeEntities db = new singspazeEntities();
            user getuser = db.user.SingleOrDefault(u => u.user_token == token);
            if (getuser != null)
                return getuser.user_id;
            else
                return 0;
        }

        public static int getview(long id,string type = "song")
        {
            singspazeEntities db = new singspazeEntities();
            int counts = 0;
            if(type == "song")
                counts = db.viewhistory.Where(v => v.Song_Id == id).Count();
            else if(type == "artist")
                counts = (from view in db.viewhistory
                         join song in db.song
                         on view.Song_Id equals song.song_id
                         join artist in db.artist
                         on song.song_artistId equals artist.artist_id
                         select artist).Count();
            return counts;
        }

        
    }

    //useful
    /// <summary>
    /// Class for get error(ex.code,detail)
    /// </summary>
    public class Errordata
    {
        /// <summary>
        /// number error
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// detail error
        /// </summary>
        public string Detail { get; set; }

    }


    /// <summary>
    /// Class for SplashPage 
    /// </summary>
    public class SplashPagedata
    {
        /// <summary>
        /// Splash Page Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string message { get; set; }

    }

    /// <summary>
    /// Class for get logindata (ex.id,token)
    /// </summary>
    [DataContract]
    public class Logindata
    {
        /// <summary>
        /// Unique string for connection
        /// </summary>
        [DataMember(Name = "token")]
        public string token { get; set; }
    }
    
    /// <summary>
    /// Class for get how many data
    /// </summary>
    [DataContract]
    public class Selectdata
    {
        /// <summary>
        /// Start data
        /// </summary>
        [DataMember(Name = "startindex")]
        public int startindex { get; set; }
        /// <summary>
        /// End data
        /// </summary>
        [DataMember(Name = "endindex")]
        public int endindex { get; set; }
    }

    //data
    /// <summary>
    /// Class data user (ex.id,username,firstname)
    /// </summary>
    public class Userdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// FacebookId
        /// </summary>
        public int fbUserId { get; set; }
        /// <summary>
        /// Firstname
        /// </summary>
        public string firstname { get; set; }
        /// <summary>
        /// Lastname
        /// </summary>
        public string lastname { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Url avatar file
        /// </summary>
        public string avatar { get; set; }
    }    
    /// <summary>
    /// Class data platlist (ex.id,description)
    /// </summary>
    public class Playlistdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }

    }

    ///// <summary>
    ///// Class data listsong data (ex.id,originname,engname)
    ///// </summary>
    //public class Listsongdata
    //{
    //    /// <summary>
    //    /// Song id
    //    /// </summary>
    //    public int id { get; set; }
    //    /// <summary>
    //    /// Origin name
    //    /// </summary>
    //    public string originName { get; set; }
    //    /// <summary>
    //    /// English name
    //    /// </summary>
    //    public string engName { get; set; }
    //    /// <summary>
    //    /// Url path for small picture
    //    /// </summary>
    //    //public string thumbnail { get; set; }
    //    /// <summary>
    //    /// Url picture
    //    /// </summary>
    //    public string URL_picture { get; set; }
    //    /// <summary>
    //    /// Price(decimal)
    //    /// </summary>
    //    public decimal? price { get; set; }
    //    /// <summary>
    //    /// View
    //    /// </summary>
    //    public int view { get; set; }
    //    /// <summary>
    //    /// length
    //    /// </summary>
    //    public string length { get; set; }
    //    /// <summary>
    //    /// Class genredata
    //    /// </summary>
    //    public Genredata genredata { get; set; }
    //    /// <summary>
    //    /// Class languagedata
    //    /// </summary>
    //    //public Languagedata languagedata { get; set; }
    //    /// <summary>
    //    /// Class albumdata
    //    /// </summary>
    //    public Albumdata albumdata { get; set; }
    //    /// <summary>
    //    /// Class artistdata
    //    /// </summary>
    //    public Artistdata artistdata { get; set; }
    //    /// <summary>
    //    /// Class contentpartnerdata
    //    /// </summary>
    //    //public Contentpartnerdata contentpartnerdata { get; set; }
    //    /// <summary>
    //    /// Class publisherdata
    //    /// </summary>
    //    public Publisherdata publisherdata { get; set; }
    //}

    /// <summary>
    /// Class data Playlistsongdata (ex.songdata,sequence)
    /// </summary>
    public class Playlistsongdata
    {
        /// <summary>
        /// Class songdata
        /// </summary>
        public Songdata songdata { get; set; }
        /// <summary>
        /// Sequence
        /// </summary>
        public int sequence { get; set; }
    }

    /// <summary>
    /// Class data song (ex.id,originName,Url)
    /// </summary>
    public class Songdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Origin name
        /// </summary>
        public string originName { get; set; }
        /// <summary>
        /// English name
        /// </summary>
        public string engName { get; set; }
        /// <summary>
        /// Url path for small picture
        /// </summary>
        //public string thumbnail { get; set; }
        /// <summary>
        /// Url picture
        /// </summary>
        public string URL_picture { get; set; }
        /// <summary>
        /// Status (1=Active,0=Deactive)
        /// </summary>
        //public int status { get; set; }
        /// <summary>
        /// FilePath (No use for now)
        /// </summary>
        //public string filePath { get; set; }
        /// <summary>
        /// Price(decimal)
        /// </summary>
        public decimal? price { get; set; }
        /// <summary>
        /// Views (All time)
        /// </summary>
        public int view { get; set; }
        /// <summary>
        /// Song length(decimal)
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// Keywords(No user for now)
        /// </summary>
        //public string keywords { get; set; }
        /// <summary>
        /// Lyrics
        /// </summary>
        public string lyrics { get; set; }
        /// <summary>
        /// ReleasedDate(only day,month,year)
        /// </summary>
        public DateTime? releasedDate { get; set; }
        /// <summary>
        /// Url iOS
        /// </summary>
        public string url_iOS { get; set; }
        /// <summary>
        /// Url Andriod and other
        /// </summary>
        public string url_Android_Other { get; set; }
        /// <summary>
        /// Url RMTP
        /// </summary>
        public string url_RTMP { get; set; }

        /// <summary>
        /// Class Genredata
        /// </summary>
        public Genredata genredata { get; set; }
        /// <summary>
        /// Class Languagedata
        /// </summary>
        //public Languagedata languagedata { get; set; }
        /// <summary>
        /// Class Albumdata
        /// </summary>
        public Albumdata albumdata { get; set; }
        /// <summary>
        /// Class Artistdata
        /// </summary>
        public Artistdata artistdata { get; set; }
        /// <summary>
        /// Class Contentpartnerdata
        /// </summary>
        //public Contentpartnerdata contentpartnerdata { get; set; }
        /// <summary>
        /// Class Publisherdata
        /// </summary>
        public Publisherdata publisherdata { get; set; }

    }

    /// <summary>
    /// Class data album (ex.id,description_TH)
    /// </summary>
    public class Albumdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Thai description
        /// </summary>
        public string description_TH { get; set; }
        /// <summary>
        /// English description
        /// </summary>
        public string description_EN { get; set; }
    }

    /// <summary>
    /// Class data listartist data (ex.id,name,picture)
    /// </summary>
    public class Listartistdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// Thai description
        /// </summary>
        public string description_TH { get; set; }
        /// <summary>
        /// English description
        /// </summary>
        public string description_EN { get; set; }
        /// /// <summary>
        /// Photo 
        /// </summary>
        public string picture { get; set; }
        /// /// <summary>
        /// Artist Type
        /// </summary>
        public string artistType { get; set; }
        /// /// <summary>
        /// View form order type (hot,name)
        /// </summary>
        public int view { get; set; }
        /// <summary>
        /// Class Publisherdata
        /// </summary>
        public Publisherdata publisherdata { get; set; }

    }

    /// <summary>
    /// Class data artist (ex.id,description)
    /// </summary>
    public class Artistdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// Thai description
        /// </summary>
        public string description_TH { get; set; }        
        /// /// <summary>
        /// English description
        /// </summary>
        public string description_EN { get; set; }
        /// /// <summary>
        /// Photo 
        /// </summary>
        public string picture { get; set; }
        /// /// <summary>
        /// Artist Type
        /// </summary>
        public string artistType { get; set; }
        /// /// <summary>
        /// Number of song in this artist
        /// </summary>
        public int songs { get; set; }   
        /// /// <summary>
        /// View (All times)
        /// </summary>
        //public int view { get; set; }
        /// <summary>
        /// Class Publisherdata
        /// </summary>
        public Publisherdata publisherdata { get; set; }
        
    }

    /// <summary>
    /// Class data contentpartner (ex.id,description)
    /// </summary>
    //public class Contentpartnerdata
    //{
    //    /// <summary>
    //    /// Id
    //    /// </summary>
    //    public int id { get; set; }
    //    /// /// <summary>
    //    /// Description
    //    /// </summary>
    //    public string description { get; set; }
    //}

    /// <summary>
    /// Class data genre (ex.id,description)
    /// </summary>
    public class Genredata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
    }

    /// <summary>
    /// Class data publisher (ex.id,description)
    /// </summary>
    public class Publisherdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
    }

    /// <summary>
    /// Class data language (ex.id,description)
    /// </summary>
    public class Languagedata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
               
    }

    /// <summary>
    /// Class data banner (ex.id,path)
    /// </summary>
    public class Bannerdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Url path
        /// </summary>
        public string path { get; set; }

    }

    /// <summary>
    /// Class data song (ex.id,originName,Url)
    /// </summary>
    public class Singhistorydata
    {
        
        /// <summary>
        /// Class songdata
        /// </summary>
        public Songdata songdata { get; set; }
        /// <summary>
        /// Sing datetime
        /// </summary>
        public DateTime singtime { get; set; }
        
    }

}