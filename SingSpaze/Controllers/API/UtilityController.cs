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
using MoonAPNS;
using System.Security.Cryptography;
using System.Security.AccessControl;


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

            db.songrequest.Add(requestdata);
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

            db.contactus.Add(contactdata);

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

        /// <summary>
        /// Get Splash Page
        /// </summary>
        /// <returns>Class O_getSplashPage</returns>
        [HttpGet]
        [ActionName("SplashPage")]
        public O_SplashPage SplashPage()
        {
            List<splashpage> splashdata = db.splashpage.Where(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).ToList();

            List<SplashPagedata> o_data = new List<SplashPagedata>();
            foreach (splashpage data in splashdata)
            {
                o_data.Add(new SplashPagedata() {
                    id = data.SplashPage_id,
                    message = data.Message,
                    url = data.URL
                });
            }


            return new O_SplashPage() { splashpagedata = o_data };
            
        }

        /// <summary>
        /// Push Notification
        /// </summary>
        /// <param name="i_data">class I_Push_Notification</param>
        /// <returns>class O_Push_Notification</returns>
        [HttpPost]
        [ActionName("Push_Notification")]
        public O_Push_Notification Push_Notification(I_Push_Notification i_data)
        {
            string errormessage = null;
            List<string> listtoken = new List<string>();
            try
            {

            //DataTable tab = datalogic.GetDeviceTokens(); // Getting all device ids from the table

            List<deviceinfo> devicedata = db.deviceinfo.ToList();

            if (!string.IsNullOrEmpty(i_data.device_token))
                devicedata = devicedata.Where(d => d.deviceInfo_deviceToken == i_data.device_token).ToList();


            var p = new List<NotificationPayload>();

          

            //for (int i = 0; i < tab.Count; i++)
            foreach (deviceinfo data in devicedata)
            {

                string message = i_data.message;
                var payload = new NotificationPayload(data.deviceInfo_deviceToken.ToString(), message, 1, "default");

                //payload.AddCustom("ID", data.user_id);
                //payload.AddCustom("ID", CardId);   // Custom fields as id and card name

                //payload.AddCustom("CardName", data.user_firstname + "" + data.user_lastname);
                //payload.AddCustom("CardName", CardName);

                p.Add(payload);

            }

            var push = new PushNotification(false, Path.Combine(HttpContext.Current.Request.MapPath("~/data/"), "Production_key.p12"), "singspaze1234");
            //var push = new PushNotification(true, Path.Combine(HttpContext.Current.Request.MapPath("~/data/"), "SingSpazeCerKey.pem"), "singspaze1234");

            //path is iphone app’s p12 certificate file, put password for that certificate also,

            var rejected = push.SendToApple(p);  // error list
            

            foreach (var item in rejected)
            {
                listtoken.Add(item);
                errormessage = errormessage +" "+ item.ToString();
                //Console.WriteLine(item);

            }

            //success;

            }

            catch (Exception ep)
            {

                errormessage = errormessage +" "+ ep.ToString();

            }

            if (string.IsNullOrEmpty(errormessage))
                return new O_Push_Notification() { result = true };
            else
                return new O_Push_Notification() { result = false, errormessage = errormessage };

            //if (listtoken.Count() == 0)
            //    return new O_Push_Notification(){ result = false,errormessage = errormessage};
            //else
            //{ 
            //    return new O_Push_Notification(){
            //        result = true,
            //        listtoken = listtoken
            //    };
            //}

        }


        /// <summary>
        /// Device Info 
        /// </summary>
        /// <param name="i_data">class I_DeviceInfo</param>
        /// <returns>class O_DeviceInfo</returns>
        [HttpPost]
        [ActionName("DeviceInfo")]
        public O_DeviceInfo DeviceInfo(I_DeviceInfo i_data)
        {
            try
            {
                db.deviceinfo.Add(new deviceinfo()
                    {
                        deviceInfo_deviceID = i_data.device_id,
                        deviceInfo_model = i_data.model,
                        deviceInfo_deviceToken = i_data.token

                    });
                db.SaveChanges();
            }
            catch
            {
                return new O_DeviceInfo()
                {
                    result = false
                };
            }

            return new O_DeviceInfo()
            {
                result = true
            };
        }

    }
}
