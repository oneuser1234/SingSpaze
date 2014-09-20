using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SingSpaze.Models;
using SingSpaze.Models.Input;
using SingSpaze.Models.Output;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Text;

namespace SingSpaze.Controllers.API
{
    /// <summary>
    /// Useful api 
    /// </summary>
    public class UtilityController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();
        /// <summary>
        /// Get Language data 
        /// </summary>
        /// <returns>List class Languagedata</returns>
        [HttpGet]
        [ActionName("Language")]
        public List<Languagedata> Language()
        {
            List<Languagedata> response = new List<Languagedata>();
            List<language> listlanguagedata = db.language.ToList();

            foreach(language data in listlanguagedata)
            {
                Languagedata o_languagedata =  new Languagedata()
                {
                    id = data.language_id,
                    description = data.language_description
                };

                response.Add(o_languagedata);
            }

            return response;            
        }
        /// <summary>
        /// Get Genre data
        /// </summary>
        /// <returns>List class Genredata</returns>
        [HttpGet]
        [ActionName("Genre")]
        public List<Genredata> Genre()
        {
            List<Genredata> response = new List<Genredata>();
            List<genre> listgenredata = db.genre.ToList();

            foreach (genre data in listgenredata)
            {
                Genredata o_genredata = new Genredata()
                {
                    id = data.genre_id,
                    description = data.genre_description
                };

                response.Add(o_genredata);
            }

            return response;
        }
        /// <summary>
        /// Get Banner data (no data for now)
        /// </summary>
        /// <returns>List class Bannerdata</returns>
        [HttpGet]
        [ActionName("Banner")]
        public List<Bannerdata> Banner()
        {
            List<Bannerdata> response = new List<Bannerdata>();
            //no data
            Bannerdata testdata = new Bannerdata()
            {
                id = 1,
                path = "no data"
            };
            response.Add(testdata);
            //List<genre> listgenredata = db.genre.ToList();

            //foreach (genre data in listgenredata)
            //{
            //    genredata o_genredata = new genredata()
            //    {
            //        id = data.genre_id,
            //        description = data.genre_description
            //    };

            //    response.Add(o_genredata);
            //}

            return response;
        }

        /// <summary>
        /// Send data to song request
        /// </summary>
        /// <param name="i_data">Class I_SongRequest</param>
        /// <returns>Class O_SongRequest</returns>
        [HttpPost]
        [ActionName("SongRequest")]
        public O_SongRequest SongRequest(I_SongRequest i_data)
        {
            if (i_data == null)
            {
                return new O_SongRequest()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            songrequest requestdata = new songrequest()
            {
                songRequest_datetime = DateTime.Now,
                songRequest_songname = i_data.songname,
                songRequest_artistname = i_data.artistname,
                user_id = Useful.getuserid(i_data.logindata.token)
            };

            db.songrequest.AddObject(requestdata);
            db.SaveChanges();

            return new O_SongRequest()
            {
                result = true
                
            };
        }

        /// <summary>
        /// Send data to contact us
        /// </summary>
        /// <param name="i_data">Class I_ContactUs</param>
        /// <returns>Class O_ContactUs</returns>
        [HttpPost]
        [ActionName("ContactUs")]
        public O_ContactUs ContactUs(I_ContactUs i_data)
        {
            if (i_data == null)
            {
                return new O_ContactUs()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            contactus contactdata = new contactus()
            {
                contactUs_datetime = DateTime.Now,
                contactUs_name = i_data.name,
                contactUs_email = i_data.email,
                contactUs_phone = i_data.phone,
                contactUs_message = i_data.message
            };

            db.contactus.AddObject(contactdata);

            //mail server
            var smtpClient = new SmtpClient("Singspaze.com")
            {
                UseDefaultCredentials = false,
                //Port = 25,
                //DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                EnableSsl = false,
                Credentials = new NetworkCredential("no-reply@singspaze.com", ";XS@}n6")
            };

            
            string message = "";
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/data/contactus.html"), Encoding.UTF8);
            try
            {
                message = sr.ReadToEnd();
                message = message.Replace("[NAME]", i_data.name);
                message = message.Replace("[EMAIL]", i_data.email);
                message = message.Replace("[PHONE]", i_data.phone);
                message = message.Replace("[MESSAGE]", i_data.message);
            }
            catch (Exception e)
            {
            }
            sr.Close();

            //send mail
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("no-reply@singspaze.com");
            mail.To.Add(new MailAddress("support@SingSpaze.com"));
            //mail.To.Add(new MailAddress("sumate@blaccess.com"));
            mail.Subject = "Contacts";
            mail.Body = message;
            mail.IsBodyHtml = true;


            smtpClient.Send(mail);


            db.SaveChanges();

            return new O_ContactUs()
            {
                result = true

            };
        }
    }
}
