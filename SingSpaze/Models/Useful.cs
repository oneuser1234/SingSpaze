﻿using System;
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
                response = "cannot use this email";
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
            else if (code_id == 16)
                response = "cannot not use this facebook ID";
            else if (code_id == 17)
                response = "server busy";

            return response;

        }

        public static Errordata geterror(int code_id)
        {
            return new Errordata()
                    {
                        code = code_id,
                        Detail = Useful.geterrordata(code_id)
                    };
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
            //no data
            if (logindata == null || string.IsNullOrEmpty(logindata.token))
                return geterror(5);

            //check 1000 => same hour same day same year
            singspazeEntities db = new singspazeEntities();
            long curactivity = db.user.Where(u => u.user_LastActivity.Hour == DateTime.Now.Hour && u.user_LastActivity.Day == DateTime.Now.Day && u.user_LastActivity.Year == DateTime.Now.Year).Count();
            if (curactivity > 1000)
                return geterror(17);

            long user_id = getuserid(logindata.token);
            if (user_id != 0)
            {
                updateactivity(user_id);
                return null;
            }
            else
                return geterror(15);

        }

        public static void updateactivity(long user_id)
        {
            singspazeEntities db = new singspazeEntities();
            user userdata = db.user.FirstOrDefault(u => u.user_id == user_id);
            userdata.user_LastActivity = DateTime.Now;
            db.SaveChanges();
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

        public static Songdata getsongdata(long id, string countrycode = "", int view = 0, string Token = null)
        {
            singspazeEntities db = new singspazeEntities();
            song datasong = db.song.Where(s => s.song_id == id).SingleOrDefault();
            if (datasong == null)
                return null;

            Songdata response = null;
            //view
            if (view == 0)
                view = Useful.getview(datasong.song_id);

            //allowedcountry
            Boolean allowedcountry = true;
            if (countrycode != "" && datasong.Song_accessRule != 0)
            {
                accessruletocountrycode access = db.accessruletocountrycode.FirstOrDefault(a => a.accessrule_id == datasong.Song_accessRule && a.allowed_countrycode == countrycode);
                if (access == null)
                    allowedcountry = false;
            }

            if (Token != null)
            {
                if (Token != "")
                    Token = "?token=" + Token;
                else
                    Token = ""; // playsong no token
                response = new Songdata()
                {
                    id = datasong.song_id,
                    engName = datasong.song_engName,
                    originName = datasong.song_originName,
                    lyrics = datasong.song_lyrics,
                    URL_picture = datasong.song_URL_picture,
                    price = datasong.song_price,
                    releasedDate = datasong.song_releasedDate,
                    //thumbnail = datasong.song_thumbnail,
                    view = view,
                    //filePath = datasong.song_filePath,
                    length = datasong.song_length,
                    //keywords = datasong.song_keywords,


                    //url
                    url_iOS = datasong.song_URL_iOS + Token,
                    url_Android_Other = datasong.song_URL_Android_Other + Token,
                    url_RTMP = datasong.song_URL_RTMP + Token,

                    //data
                    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
                    albumdata = Useful.getalbumdata(datasong.song_albumId),
                    artistdata = Useful.getartistdata(datasong.song_artistId),
                    genredata = Useful.getgenredata(datasong.song_genre),
                    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId),

                    allowedcountry = allowedcountry
                };
            }
            else
            {
                response = new Songdata()
                {
                    id = datasong.song_id,
                    engName = datasong.song_engName,
                    originName = datasong.song_originName,
                    lyrics = datasong.song_lyrics,
                    URL_picture = datasong.song_URL_picture,
                    price = datasong.song_price,
                    releasedDate = datasong.song_releasedDate,
                    //thumbnail = datasong.song_thumbnail,
                    view = view,
                    //filePath = datasong.song_filePath,
                    length = datasong.song_length,
                    //keywords = datasong.song_keywords,

                    //data
                    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
                    albumdata = Useful.getalbumdata(datasong.song_albumId),
                    artistdata = Useful.getartistdata(datasong.song_artistId),
                    genredata = Useful.getgenredata(datasong.song_genre),
                    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId),

                    allowedcountry = allowedcountry
                };
            }

            return response;

        }

        public static Songdata getsongdatanolyrics(long id,string countrycode = "", int view = 0, string Token = null)
        {
            singspazeEntities db = new singspazeEntities();
            song datasong = db.song.Where(s => s.song_id == id).SingleOrDefault();
            if (datasong == null)
                return null;

            Songdata response = null;
            //view
            if (view == 0)
                view = Useful.getview(datasong.song_id);

            //allowedcountry
            Boolean allowedcountry = true;
            if (countrycode != "" && datasong.Song_accessRule != 0)
            {
                accessruletocountrycode access = db.accessruletocountrycode.FirstOrDefault(a => a.accessrule_id == datasong.Song_accessRule && a.allowed_countrycode == countrycode);
                if (access == null)
                    allowedcountry = false;
            }

            if (Token != null)
            {
                if (Token != "")
                    Token = "?token=" + Token;
                else
                    Token = ""; // playsong no token
                response = new Songdata()
                {
                    id = datasong.song_id,
                    engName = datasong.song_engName,
                    originName = datasong.song_originName,
                    //lyrics = datasong.song_lyrics,
                    URL_picture = datasong.song_URL_picture,
                    price = datasong.song_price,
                    releasedDate = datasong.song_releasedDate,
                    //thumbnail = datasong.song_thumbnail,
                    view = view,
                    //filePath = datasong.song_filePath,
                    length = datasong.song_length,
                    //keywords = datasong.song_keywords,


                    //url
                    url_iOS = datasong.song_URL_iOS + Token,
                    url_Android_Other = datasong.song_URL_Android_Other + Token,
                    url_RTMP = datasong.song_URL_RTMP + Token,

                    //data
                    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
                    albumdata = Useful.getalbumdata(datasong.song_albumId),
                    artistdata = Useful.getartistdata(datasong.song_artistId),
                    genredata = Useful.getgenredata(datasong.song_genre),
                    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId),

                    allowedcountry = allowedcountry
                };
            }
            else
            {
                response = new Songdata()
                {
                    id = datasong.song_id,
                    engName = datasong.song_engName,
                    originName = datasong.song_originName,
                    //lyrics = datasong.song_lyrics,
                    URL_picture = datasong.song_URL_picture,
                    price = datasong.song_price,
                    releasedDate = datasong.song_releasedDate,
                    //thumbnail = datasong.song_thumbnail,
                    view = view,
                    //filePath = datasong.song_filePath,
                    length = datasong.song_length,
                    //keywords = datasong.song_keywords,

                    //data
                    //languagedata = Useful.getlanguagedata(datasong.song_languageId),
                    albumdata = Useful.getalbumdata(datasong.song_albumId),
                    artistdata = Useful.getartistdata(datasong.song_artistId),
                    genredata = Useful.getgenredata(datasong.song_genre),
                    publisherdata = Useful.getpublishersongdata(datasong.publisherforsong_id),
                    //contentpartnerdata = Useful.getcontentpartnerdata(datasong.song_contentPartnerId),

                    allowedcountry = allowedcountry
                };
            }

            return response;

        }

        public static Albumdata getalbumdata(long id)
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

        public static Artistdata getartistdata(long id,int view = 0)
        {
            singspazeEntities db = new singspazeEntities();
            artist data = db.artist.Where(a => a.artist_id == id).SingleOrDefault();
            if (data == null)
                return null;
            if (view == 0)
                view = Useful.getview(id, "artist");
            return new Artistdata()
            {
                id = id,
                description_TH = data.artist_description_th,
                description_EN = data.artist_description_en,
                picture = data.artist_picture,
                picture_l_lo = data.ArtistPicture_L_Lo,
                picture_l_hi = data.ArtistPicture_L_Hi,
                picture_s_lo = data.ArtistPicture_S_Lo,
                picture_s_hi = data.ArtistPicture_S_Hi,
                view = view,
                artistType = data.artist_type,
                totalsongs = db.song.Where(s => s.song_artistId == data.artist_id).Count(),
                publisherdata = Useful.getpublisherartistdata(data.artist_publisherforartistId)
            };

        }

        public static Genredata getgenredata(long id)
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

        public static Publisherdata getpublishersongdata(long id)
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

        public static Publisherdata getpublisherartistdata(long id)
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

        public static long getuserid(string token)
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
                         where artist.artist_id == id
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
        public long id { get; set; }
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
        public long id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// FacebookId
        /// </summary>
        public string fbUserId { get; set; }
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

        /// <summary>
        /// Register date
        /// </summary>
        public DateTime registerdate { get; set; }
         
    }    
    /// <summary>
    /// Class data platlist (ex.id,description)
    /// </summary>
    public class Playlistdata
    {
        /// <summary>
        /// Id
        /// </summary>
        public long id { get; set; }
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
        public long id { get; set; }
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
        /// <summary>
        /// Allowed country
        /// </summary>
        public Boolean allowedcountry { get; set; }
         
    }

   


    /// <summary>
    /// Class data album (ex.id,description_TH)
    /// </summary>
    public class Albumdata
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
    }

    /// <summary>
    /// Class data listartist data (ex.id,name,picture)
    /// </summary>
    //public class Listartistdata
    //{
    //    /// <summary>
    //    /// Id
    //    /// </summary>
    //    public long id { get; set; }
    //    /// <summary>
    //    /// Thai description
    //    /// </summary>
    //    public string description_TH { get; set; }
    //    /// <summary>
    //    /// English description
    //    /// </summary>
    //    public string description_EN { get; set; }
    //    /// /// <summary>
    //    /// Photo 
    //    /// </summary>
    //    public string picture { get; set; }
    //    /// /// <summary>
    //    /// Artist Type
    //    /// </summary>
    //    public string artistType { get; set; }
    //    /// /// <summary>
    //    /// View form order type (hot,name)
    //    /// </summary>
    //    public int view { get; set; }
    //    /// <summary>
    //    /// Class Publisherdata
    //    /// </summary>
    //    public Publisherdata publisherdata { get; set; }

    //}

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
        /// Big picture low resolution 
        /// </summary>
        public string picture_l_lo { get; set; }
        /// /// <summary>
        /// Big picture high resolution  
        /// </summary>
        public string picture_l_hi { get; set; }
        /// /// <summary>
        /// Small picture low resolution  
        /// </summary>
        public string picture_s_lo { get; set; }
        /// /// <summary>
        /// Small picture high resolution 
        /// </summary>
        public string picture_s_hi { get; set; }
        /// /// <summary>
        /// Artist Type
        /// </summary>
        public string artistType { get; set; }
        /// /// <summary>
        /// Number of song in this artist
        /// </summary>
        public int totalsongs { get; set; }   
        /// /// <summary>
        /// View 
        /// </summary>
        public int view { get; set; }
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
        public long id { get; set; }
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
        public long id { get; set; }
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
        public long id { get; set; }
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
    /// Class data historysong (ex.id,originName,Url)
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

    /// <summary>
    /// Class data record (ex.id,url)
    /// </summary>
    public class Recorddata
    {
        /// <summary>
        /// Id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// Class songdata
        /// </summary>
        public Songdata songdata { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Sing datetime
        /// </summary>
        public DateTime recordtime { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string url { get; set; }

    }

}