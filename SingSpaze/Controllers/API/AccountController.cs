using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SingSpaze.Models;
using System.Web.Security;
using WebMatrix.WebData;
using SingSpaze.Models.Output;
using System.Security.Cryptography;
using System.Text;
using SingSpaze.Models.Input;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;



namespace SingSpaze.Controllers.API
{
    /// <summary>
    /// Account api
    /// </summary>
    public class AccountController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        /// <summary>
        /// Send data for register (ex.username,password,firstname,facebookId)
        /// </summary>
        /// <param name="i_data">class I_Register</param>
        /// <returns>class O_Register</returns>
        [HttpPost]
        [ActionName("Register")]
        public O_Register register(I_Register i_data)
        {

            if (i_data == null || string.IsNullOrEmpty(i_data.Firstname) || string.IsNullOrEmpty(i_data.Lastname) || string.IsNullOrEmpty(i_data.Email))
            {
                return new O_Register()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }
                user checkuser = new user();
                checkuser = db.user.FirstOrDefault(u => u.user_fbUserId == 0 && u.user_email == i_data.Email);
               

                if (checkuser != null)
                {
                    return new O_Register()
                    {
                        result = false,
                        errordata = new Errordata()
                                    {
                                        code = 2,
                                        Detail = Useful.geterrordata(2)
                                    } 
                    };
                }
                /// end
                
                user userdata = new user()
                {
                    user_firstname = i_data.Firstname,
                    user_lastname = i_data.Lastname,
                    user_password = i_data.Password,
                    user_email = i_data.Email,
                    user_createdDatetime = DateTime.Now,

                    user_avartar = null,
                    user_lastlogin = null,
                    user_modifiedDatetime = null,
                    user_status = 1,
                    user_type = 0,
                    usergroup_id = 0
                };

                db.user.Add(userdata);

                //mail server
                var smtpClient = new SmtpClient("Singspaze.com")
                {
                    UseDefaultCredentials = false,
                    //Port = 25,
                    //DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    EnableSsl = false,
                    Credentials = new NetworkCredential("no-reply@singspaze.com", ";XS@}n6")
                };

                string url = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] + ":" + HttpContext.Current.Request.ServerVariables["server_port"];
                string message = "";
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/data/welcome.html"), Encoding.UTF8);
                try
                {
                    message = sr.ReadToEnd();
                    message = message.Replace("[URL]", url);
                    message = message.Replace("[EMAIL]", userdata.user_email);
                    message = message.Replace("[NAME]", userdata.user_firstname + " " + userdata.user_lastname);
                }
                catch (Exception e)
                {
                }
                sr.Close();

                //send mail
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress("no-reply@singspaze.com");
                mail.To.Add(new MailAddress(userdata.user_email));
                mail.Subject = "Thank you for confirming your registration at SingSpaze";
                mail.Body = message;
                mail.IsBodyHtml = true;

                //smtpClient.Send(mail);
                db.SaveChanges();

                return new O_Register()
                    {
                        result = true
                    };
            //}
            //else
           // {
                
           //     return new O_Register()
           //     {
           //         result = false,
           //         errordata = new errordata()
           //         {
           //             code = 1,
           //             Detail = Useful.geterrordata(1)
           //         }
           //     };
            //}
        }

        private O_FBRegister FBregister(I_FBRegister i_data)
        {
           
            if (i_data == null || string.IsNullOrEmpty(i_data.Lastname) || string.IsNullOrEmpty(i_data.Email))
            {
                return new O_FBRegister()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }
            

            
            user checkuser = new user();
            checkuser = db.user.FirstOrDefault(u => u.user_fbUserId == i_data.fbUserId);

            if (checkuser != null)
            {
                return new O_FBRegister()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 2,
                        Detail = Useful.geterrordata(2)
                    }
                };
            }
            /// end

            user userdata = new user()
            {
                user_fbUserId = i_data.fbUserId,
                user_firstname = i_data.Firstname,
                user_lastname = i_data.Lastname,
                user_login = "",
                user_password = "",
                user_email = i_data.Email,
                user_createdDatetime = DateTime.Now,

                user_avartar = null,
                user_lastlogin = null,
                user_modifiedDatetime = null,
                user_status = 1,
                user_type = 0,
                usergroup_id = 0
            };
            db.user.Add(userdata);

            //mail server
            var smtpClient = new SmtpClient("Singspaze.com")
            {
                UseDefaultCredentials = false,
                //Port = 25,
                //DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                EnableSsl = false,
                Credentials = new NetworkCredential("no-reply@singspaze.com", ";XS@}n6")
            };

            string url = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] + ":" + HttpContext.Current.Request.ServerVariables["server_port"];
            string message = "";
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/data/welcome.html"), Encoding.UTF8);
            try
            {
                message = sr.ReadToEnd();
                message = message.Replace("[URL]", url);
                message = message.Replace("[EMAIL]", userdata.user_email);
                message = message.Replace("[NAME]", userdata.user_firstname + " " + userdata.user_lastname);
            }
            catch (Exception e)
            {
            }
            sr.Close();

            //send mail
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("no-reply@singspaze.com");
            mail.To.Add(new MailAddress(userdata.user_email));
            mail.Subject = "Thank you for confirming your registration at SingSpaze";
            mail.Body = message;
            mail.IsBodyHtml = true;

            smtpClient.Send(mail);
            db.SaveChanges();

            return new O_FBRegister()
            {
                result = true
            };
            
        }

        /// <summary>
        /// Send data for login(ex.username,password)
        /// </summary>
        /// <param name="i_data">Class I_Login</param>
        /// <returns>Class O_Login</returns>        
        [HttpPost]
        [ActionName("Login")]
        public O_Login Login(I_Login i_data)
        {
                       
            user datauser = new user();
            if (i_data == null || string.IsNullOrEmpty(i_data.email) || string.IsNullOrEmpty(i_data.password) || string.IsNullOrEmpty(i_data.Mac_Address) || string.IsNullOrEmpty(i_data.Device_ID))
            {
                return new O_Login()
                {
                    errordata = new Errordata()
                    {
                        code = 3,
                        Detail = Useful.geterrordata(3)
                    }
                };
            }
            //normal
            //if (string.IsNullOrEmpty(i_data.Username) || string.IsNullOrEmpty(i_data.Password))
            //        errorinput = true;

            datauser = db.user.FirstOrDefault(u => u.user_email == i_data.email && u.user_password == i_data.password && u.user_fbUserId == 0);
            
          
           if (datauser == null)
           {
               return new O_Login()
               {
                   errordata = new Errordata()
                   {
                       code = 4,
                       Detail = Useful.geterrordata(4)
                   }
               };
           }
           else
           {
               //date + mac + device + sing (md5)
               string Token = Useful.GetMd5Hash(MD5.Create(), DateTime.Now.ToString() + i_data.Mac_Address.ToString() + i_data.Device_ID.ToString() + "sing");
               datauser.user_lastlogin = DateTime.Now;
               datauser.user_token = Token;
               datauser.APNS_token = i_data.Device_ID;
               db.SaveChanges();
               //datauser 
               return new O_Login()
               {
                   Token = Token
               };
           }
        }

        /// <summary>
        /// Send data for FBlogin(ex.fbuserid,firstname)
        /// </summary>
        /// <param name="i_data">Class I_Login</param>
        /// <returns>Class O_Login</returns>
        [HttpPost]
        [ActionName("FBlogin")]
        public O_FBLogin FBlogin(I_FBLogin i_data)
        {
            user datauser = new user();
            if (i_data == null || string.IsNullOrEmpty(i_data.Email))
            {
                return new O_FBLogin()
                {
                    errordata = new Errordata()
                    {
                        code = 3,
                        Detail = Useful.geterrordata(3)
                    }
                };
            
            }
            
                user checkfacebook = db.user.FirstOrDefault(u => u.user_email == i_data.Email && u.user_fbUserId == i_data.fbUserId);
                if (checkfacebook == null) //no databefore => new register
                {
                    I_FBRegister newregister = new I_FBRegister()
                    {
                        fbUserId = i_data.fbUserId,
                        Email = i_data.Email,
                        Firstname = i_data.Firstname,
                        Lastname = i_data.Lastname
                    };

                    O_FBRegister response = FBregister(newregister);
                    if (!response.result) // cannot register
                    {
                        return new O_FBLogin()
                        {
                            errordata = response.errordata
                        };
                    }
                }

                datauser = db.user.FirstOrDefault(u => u.user_fbUserId == i_data.fbUserId && u.user_email == i_data.Email);
            
            if (datauser == null)
            {
                return new O_FBLogin()
                {
                    errordata = new Errordata()
                    {
                        code = 4,
                        Detail = Useful.geterrordata(4)
                    }
                };
            }
            else
            {
                string Token = Useful.GetMd5Hash(MD5.Create(), DateTime.Now.ToString() + i_data.Mac_Address.ToString() + i_data.Device_ID.ToString() + "sing");
                datauser.user_lastlogin = DateTime.Now;
                datauser.user_token = Token;
                datauser.APNS_token = i_data.Device_ID;
                db.SaveChanges();
                return new O_FBLogin()
                {
                    Token = Token
                };
            }
        }

        

        /// <summary>
        /// Profile
        /// </summary>
        /// <param name="i_data">Class I_Profile</param>
        /// <returns>Clas O_Profile</returns>
        [HttpPost]
        [ActionName("Profile")]
        public O_Profile Profile(I_Profile i_data)
        {
            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_Profile()
                       {
                           errordata = Useful.checklogin(i_data.logindata)
                       };
            }
            
            user userdata = db.user.SingleOrDefault(u => u.user_token == i_data.logindata.token);

            if (userdata == null)
            {
                return new O_Profile()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }
            else
            {
                
                return new O_Profile()
                {
                    userdata =  new Userdata()
                    {
                        id = userdata.user_id,
                        email = userdata.user_email,
                        fbUserId = userdata.user_fbUserId,
                        firstname = userdata.user_firstname,
                        lastname = userdata.user_lastname,
                        username = userdata.user_login,
                        avatar = userdata.user_avartar
                    }
                };
            }
        }

        
        /// <summary>
        /// Send data to change user data (Facebook account cannot use this)
        /// </summary>
        /// <param name="i_data">Class I_EditProfile</param>
        /// <returns>Class O_EditProfile</returns>
        [HttpPut]
        [ActionName("EditProfile")]
        public O_EditProfile EditProfile(I_EditProfile i_data)//user_id
        {
            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_EditProfile()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }

            Boolean errorinput = false;
            if(i_data == null)
                errorinput = true;

            if(string.IsNullOrEmpty(i_data.Email) || string.IsNullOrEmpty(i_data.Firstname) || string.IsNullOrEmpty(i_data.Lastname) )
                errorinput = true;
            if(!string.IsNullOrEmpty(i_data.newPassword) && string.IsNullOrEmpty(i_data.oldPassword) ) // when change password
                errorinput = true;

            if(errorinput)
            {
                return new O_EditProfile()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 9,
                        Detail = Useful.geterrordata(9)
                    }
                };

            }

                user edituser = db.user.SingleOrDefault(u => u.user_token == i_data.logindata.token);
                if(edituser != null)
                {

                    if (edituser.user_fbUserId != 0) //facebook
                    {
                        return new O_EditProfile()
                        {
                            result = false,
                            errordata = new Errordata()
                            {
                                code = 13,
                                Detail = Useful.geterrordata(13)
                            }
                        };
                    }

                    user checkuser = db.user.FirstOrDefault(u => u.user_email == i_data.Email);
                    if (checkuser != null)
                    {
                        return new O_EditProfile()
                        {
                            result = false,
                            errordata = new Errordata()
                            {
                                code = 2,
                                Detail = Useful.geterrordata(2)
                            }
                        };

                    }

                    if (edituser.user_password != i_data.oldPassword)
                    {
                        return new O_EditProfile()
                        {
                            result = false,
                            errordata = new Errordata()
                            {
                                code = 7,
                                Detail = Useful.geterrordata(7)
                            }
                        };
                    }

                    edituser.user_firstname = i_data.Firstname;
                    edituser.user_lastname = i_data.Lastname;
                    edituser.user_password = i_data.newPassword;
                    edituser.user_email = i_data.Email;
                    edituser.user_modifiedDatetime = DateTime.Now;

                    db.SaveChanges();
                    return new O_EditProfile()
                    {
                        result = true                        
                    };
                }

                else
                {
                    return new O_EditProfile()
                    {
                        result = false,
                        errordata = new Errordata()
                        {
                            code = 8,
                            Detail = Useful.geterrordata(8)
                        }
                    };
                }

                
            
            
        }

        /// <summary>
        /// Send data for set new password to email (Facebook account cannot use this)
        /// </summary>
        /// <param name="i_data">Class I_Forgot</param>
        /// <returns>Class O_Forgot</returns>
        [HttpPost]
        [ActionName("Forgot")]
        public O_Forgot Forgot(I_Forgot i_data)
        {
            user userdata = db.user.Where(u => u.user_email == i_data.email).SingleOrDefault();

            if (userdata == null)
            {
                return new O_Forgot()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            if (userdata.user_fbUserId != 0)
            {
                return new O_Forgot()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 13,
                        Detail = Useful.geterrordata(13)
                    }
                };
            }

            /// time + reset md5
            string Retoken = Useful.GetMd5Hash(MD5.Create(), DateTime.Now.ToString()  + "reset");
            userdata.user_retoken = Retoken;

            //mail server
            var smtpClient = new SmtpClient("Singspaze.com")
            {
                UseDefaultCredentials = false,
                //Port = 25,
                //DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                EnableSsl = false,
                Credentials = new NetworkCredential("no-reply@singspaze.com", ";XS@}n6")
            };

            string url = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] + ":" + HttpContext.Current.Request.ServerVariables["server_port"] + @"/api/account/reset?token=" + Retoken + "&email="+userdata.user_email;
            string message = "";
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/data/forgot.html"), Encoding.UTF8);
            try
            {
                message = sr.ReadToEnd();
                message = message.Replace("[URL]", url);
            }
            catch (Exception e)
            {
            }
            sr.Close();

            //send mail
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("no-reply@singspaze.com");
            mail.To.Add(new MailAddress(userdata.user_email));
            mail.Subject = "Forgot password";
            mail.Body = message;
            mail.IsBodyHtml = true;

            
            smtpClient.Send(mail);
            db.SaveChanges();

            



            return new O_Forgot()
            {
                result = true
            };
            
            
        }

        /// <summary>
        /// Send data with reset Token to set new password (Facebook account cannot use this)
        /// </summary>
        /// <param name="i_data">Class I_Reset</param>
        /// <returns>Class O_Reset</returns>
        [HttpPost]
        [ActionName("Reset")]
        public O_Reset Reset(I_Reset i_data)
        {
            if (i_data == null || string.IsNullOrEmpty(i_data.retoken))
            {
                return new O_Reset()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            user edituser = db.user.Where(u => u.user_retoken == i_data.retoken).SingleOrDefault();
            if (edituser != null)
            {
               

                edituser.user_password = i_data.newpassword;
                edituser.user_retoken = null;

                //mail server
                var smtpClient = new SmtpClient("Singspaze.com")
                {
                    UseDefaultCredentials = false,
                    //Port = 25,
                    //DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    EnableSsl = false,
                    Credentials = new NetworkCredential("no-reply@singspaze.com", ";XS@}n6")
                };

                string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string time = DateTime.Now.ToString();
                string message = "";
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/data/reset.html"), Encoding.UTF8);
                try
                {
                    message = sr.ReadToEnd();
                    message = message.Replace("[TIME]", time);
                    message = message.Replace("[IP]", ip);
                }
                catch (Exception e)
                {
                }
                sr.Close();

                //send mail
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress("no-reply@singspaze.com");
                mail.To.Add(new MailAddress(edituser.user_email));
                mail.Subject = "Reset password";
                mail.Body = message;
                mail.IsBodyHtml = true;

                smtpClient.Send(mail);

                db.SaveChanges();

                return new O_Reset()
                {
                    result = true
                };
            }
            else
            {
                return new O_Reset()
                {
                    result = false,
                    errordata = new Errordata()
                    {
                        code = 14,
                        Detail = Useful.geterrordata(14)
                    }
                };
            }
            
        }


        /// <summary>
        /// Upload Profile Picture
        /// </summary>
        /// <returns>Class O_upload</returns>
        [HttpPost]
        [ActionName("UploadProfilePic")]
        public O_Upload UploadProfilePic()
        {
            Logindata logindata = new Logindata();
            var httpRequest = HttpContext.Current.Request;


            if (!Request.Content.IsMimeMultipartContent())
            {
                return new O_Upload()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }


            foreach (var val in httpRequest.Form.GetValues("token"))
                {
                    logindata = new Logindata()
                            {
                                token = val
                            };                    
                }
            


            if (Useful.checklogin(logindata) != null)
            {
                return new O_Upload()
                {
                    errordata = Useful.checklogin(logindata)
                };
            }

            user userdata = db.user.SingleOrDefault(u => u.user_token == logindata.token);

            if (userdata == null)
            {
                return new O_Upload()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            if (httpRequest.Files.Count > 0)
            {
                
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string filename = userdata.user_id + "_profile_256_256.jpg";
                    var filePath = HttpContext.Current.Server.MapPath("~/Picture/" + filename);
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);

                    //add database
                    userdata.user_avartar = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] + ":" + HttpContext.Current.Request.ServerVariables["server_port"] + @"/Picture/" + filename;

                    db.SaveChanges();
                }
                return new O_Upload()
                {
                    result = true   
                };
            }
            else
            {
                return new O_Upload()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }
            
        }

        
        
    }
}