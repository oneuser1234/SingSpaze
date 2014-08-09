using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using SingSpaze.Models.Input;

namespace SingSpaze.Models
{
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

        public static Boolean checklogin(logindata logindata)
        {
            if (logindata == null || string.IsNullOrEmpty(logindata.id.ToString()) || string.IsNullOrEmpty(logindata.token))
                return false;
            
            if (VerifyMd5Hash(MD5.Create(), logindata.id.ToString() + "sing", logindata.token))
                return true;
            else
                return false;

        }

        public static selectdata getbaseselectdata()
        {
            return new selectdata()
            {
                skip = 0,
                take = 5
            };
        }

        public static List<int> getlistdata(string data)
        {
            List<int> response = new List<int>();
            string[] arraydata = data.Split(',');

            foreach (string stringdata in arraydata)
                response.Add(Int32.Parse(stringdata));

            return response;
        }

        public static albumdata getalbumdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            album data = db.album.Where(a => a.album_id == id).SingleOrDefault();
            return new albumdata()
            {
                id = id,
                description_TH = data.album_description_th,
                description_EN = data.album_description_en
            };
                       
        }

        public static artistdata getartistdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            artist data = db.artist.Where(a => a.artist_id == id).SingleOrDefault();
            return new artistdata()
            {
                id = id,
                description_TH = data.artist_description_th,
                description_EN = data.artist_description_en
            };

        }

        public static genredata getgenredata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            genre data = db.genre.Where(a => a.genre_id == id).SingleOrDefault();
            return new genredata()
            {
                id = id,
                description = data.genre_description
            };

        }

        public static recordlabeldata getrecordlabeldata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            recordlabel data = db.recordlabel.Where(a => a.recordlabel_id == id).SingleOrDefault();
            return new recordlabeldata()
            {
                id = id,
                description = data.recordlabel_description
            };

        }

        public static contentpartnerdata getcontentpartnerdata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            contentpartner data = db.contentpartner.Where(a => a.contentpartner_id == id).SingleOrDefault();
            return new contentpartnerdata()
            {
                id = id,
                description = data.contentpartner_description
            };

        }

        public static languagedata getlanguagedata(int id)
        {
            singspazeEntities db = new singspazeEntities();
            language data = db.language.Where(a => a.language_id == id).SingleOrDefault();
            return new languagedata()
            {
                id = id,
                description = data.language_description
            };

        }
        
    }

    //useful
    public class errordata
    {
        public int code { get; set; }
        public string Detail { get; set; }

    }

    public class logindata
    {
        public int id { get; set; }
        public string token { get; set; }
    }
    
    public class selectdata
    {
        public int skip { get; set; }
        public int take { get; set; }
    }

    //data
    public class userdata
    {
        public int id { get; set; }
        public string username { get; set; }
        public int fbUserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string avatar { get; set; }
    }    

    public class playlistdata
    {
        public int id { get; set; }
        public string description { get; set; }

    }

    public class listsongdata
    {
        public int id { get; set; }
        public string originName { get; set; }
        public string engName { get; set; }       
        public string thumbnail { get; set; }
        public string picture { get; set; }      
        public decimal? price { get; set; }
        public int? view { get; set; }

        public genredata genredata { get; set; }
        public languagedata languagedata { get; set; }
        public albumdata albumdata { get; set; }
        public artistdata artistdata { get; set; }
        public contentpartnerdata contentpartnerdata { get; set; }
        public recordlabeldata recordlabeldata { get; set; }
    }

    public class songdata
    {
        public int id { get; set; }
        public string originName { get; set; }
        public string engName { get; set; }        
        public string thumbnail { get; set; }
        public string picture { get; set; }
        public int status { get; set; }
        public string filePath { get; set; }
        public decimal? price { get; set; }
        public int? view { get; set; }
        public decimal length { get; set; }
        public string keywords { get; set; }
        public string lyrics { get; set; }
        public DateTime? releasedDate { get; set; }

        public genredata genredata { get; set; }
        public languagedata languagedata { get; set; }
        public albumdata albumdata { get; set; }
        public artistdata artistdata { get; set; }
        public contentpartnerdata contentpartnerdata { get; set; }
        public recordlabeldata recordlabeldata { get; set; }

    }

    public class albumdata
    {
        public int id { get; set; }
        public string description_TH { get; set; }
        public string description_EN { get; set; }
    }

    public class artistdata
    {
        public int id { get; set; }
        public string description_TH { get; set; }
        public string description_EN { get; set; }
    }

    public class contentpartnerdata
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class genredata
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class recordlabeldata
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class languagedata
    {
        public int id { get; set; }
        public string description { get; set; }
               
    }

    public class bannerdata
    {
        public int id { get; set; }
        public string path { get; set; }

    }
    


}