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
using SingSpaze.Models.Parameter;


namespace SingSpaze.Controllers.API
{
    public class AccountController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="i_data">class I_Register</param>
        /// <returns>class O_Register</returns>
        [HttpPost]
        [ActionName("Register")]
        public O_Register Register(I_Register i_data)
        {
            /// testttt
            //if (ModelState.IsValid)
            //{
            Boolean errorinput = false;
            if (i_data == null)
                errorinput = true;
            if (i_data.fbUserId == 0)
            {
                if(string.IsNullOrEmpty(i_data.Username) || string.IsNullOrEmpty(i_data.Password))
                    errorinput = true;
            }

            if (errorinput || string.IsNullOrEmpty(i_data.Firstname) || string.IsNullOrEmpty(i_data.Lastname) || string.IsNullOrEmpty(i_data.Email))
            {
                return new O_Register()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }
                user checkuser = new user();
                if(i_data.fbUserId == 0)
                    checkuser = db.user.FirstOrDefault(u => u.user_login == i_data.Username || u.user_email == i_data.Email);
                else
                    checkuser = db.user.FirstOrDefault(u => u.user_fbUserId == i_data.fbUserId || u.user_email == i_data.Email);

                if (checkuser != null)
                {
                    return new O_Register()
                    {
                        result = false,
                        errordata = new errordata()
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
                    user_login = i_data.Username,
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
                db.user.AddObject(userdata);
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

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="i_data">class I_login</param>
        /// <returns>class O_login</returns>
        [HttpPost]
        [ActionName("Login")]
        public O_Login Login(I_Login i_data)
        {
            Boolean errorinput = false;            
            user datauser = new user();
            if (i_data == null)
                errorinput = true;
            //normal
            if (i_data.fbUserId == 0)
            {
                if (string.IsNullOrEmpty(i_data.Username) || string.IsNullOrEmpty(i_data.Password))
                    errorinput = true;

                datauser = db.user.FirstOrDefault(u => u.user_login == i_data.Username && u.user_password == i_data.Password);
            }
            else  //facebook
            {
                user checkfacebook = db.user.FirstOrDefault(u => u.user_email == i_data.Email && u.user_fbUserId == i_data.fbUserId);
                if (checkfacebook == null) //no databefore => new register
                {
                    I_Register newregister = new I_Register()
                    {
                        fbUserId = i_data.fbUserId,
                        Email = i_data.Email,
                        Firstname = i_data.Firstname,
                        Lastname = i_data.Lastname
                    };

                    O_Register response = Register(newregister);
                    if (!response.result) // cannot register
                        errorinput = true;
                }

                datauser = db.user.FirstOrDefault(u => u.user_fbUserId == i_data.fbUserId && u.user_email == i_data.Email);
            }

            if (errorinput)
            {
                return new O_Login()
                {
                    errordata = new errordata()
                    {
                        code = 3,
                        Detail = Useful.geterrordata(3)
                    }
                };
            }

           
           
                       

          
           if (datauser == null)
           {
               return new O_Login()
               {
                   errordata = new errordata()
                   {
                       code = 4,
                       Detail = Useful.geterrordata(4)
                   }
               };
           }
           else
           {
               //FormsAuthentication.SetAuthCookie(datauser.user_login, true);
               return new O_Login()
               {
                   Id = datauser.user_id,
                   Token = Useful.GetMd5Hash(MD5.Create(), datauser.user_id.ToString() + "sing")
               };
           }
        }

        //[Authorize]
        //[HttpGet]
        //[ActionName("Logout")]
        //public void Logout()
        //{
        //  FormsAuthentication.SignOut();
        //}


        //[Authorize]
        
        //[HttpPost]
        //[ActionName("Profile")]
        //public O_ListProfile ListProfile(I_ListProfile i_data)
        //{
            
        //    if (Useful.checklogin(i_data.logindata))
        //    {

        //        List<user> listuserdata = db.user.OrderBy(u => u.user_id).Skip(i_data.selectdata.skip).Take(i_data.selectdata.take).ToList();

        //        List<userdata> o_listdata = new List<userdata>();

               

        //        foreach (user data in listuserdata)
        //        {
        //            userdata userdata = new userdata();
        //            userdata.Email = data.user_email;
        //            userdata.fbUserId = data.user_fbUserId;
        //            userdata.Firstname = data.user_firstname;
        //            userdata.Lastname = data.user_lastname;
        //            userdata.username = data.user_login;

        //            o_listdata.Add(userdata);
        //        }

        //        return new O_ListProfile()
        //        {
        //            userdata = o_listdata.AsEnumerable()
        //        };
        //    }
        //    else
        //    {
        //        return new O_ListProfile()
        //               {
        //                   errordata = new errordata()
        //                   {
        //                       code = 5,
        //                       Detail = Useful.geterrordata(5)
        //                   }
        //               };
        //    }
        //}


        //[Authorize]

        /// <summary>
        /// Profile
        /// </summary>
        /// <param name="i_data">class I_Profile</param>
        /// <returns>clas O_Profile</returns>
        [HttpPost]
        [ActionName("Profile")]
        public O_Profile Profile(I_Profile i_data)
        {
            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_Profile()
                       {
                           errordata = new errordata()
                           {
                               code = 5,
                               Detail = Useful.geterrordata(5)
                           }
                       };
            }
            
            user userdata = db.user.SingleOrDefault(u => u.user_id == i_data.logindata.id);

            if (userdata == null)
            {
                return new O_Profile()
                {
                    errordata = new errordata()
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
                    userdata =  new userdata()
                    {
                        id = userdata.user_id,
                        Email = userdata.user_email,
                        fbUserId = userdata.user_fbUserId,
                        Firstname = userdata.user_firstname,
                        Lastname = userdata.user_lastname,
                        username = userdata.user_login,
                        avatar = userdata.user_avartar
                    }
                };
            }
        }

        //[Authorize]
        [HttpPut]
        [ActionName("Edit")]
        public O_EditProfile EditProfile(I_EditProfile i_data)//user_id
        {
            if (!Useful.checklogin(i_data.logindata))
            {
                return new O_EditProfile()
                {
                    errordata = new errordata()
                    {
                        code = 5,
                        Detail = Useful.geterrordata(5)
                    }
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
                    errordata = new errordata()
                    {
                        code = 9,
                        Detail = Useful.geterrordata(9)
                    }
                };

            }

                user edituser = db.user.SingleOrDefault(u => u.user_id == i_data.logindata.id);
                if(edituser != null)
                {

                    if (edituser.user_fbUserId != 0) //facebook
                    {
                        return new O_EditProfile()
                        {
                            result = false,
                            errordata = new errordata()
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
                            errordata = new errordata()
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
                            errordata = new errordata()
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
                        errordata = new errordata()
                        {
                            code = 8,
                            Detail = Useful.geterrordata(8)
                        }
                    };
                }

                
            
            
        }


        [HttpPost]
        [ActionName("Forgot")]
        public O_Forgot Forgot(I_Forgot i_data)
        {
            user userdata = db.user.Where(u => u.user_email == i_data.email).SingleOrDefault();

            if (userdata == null)
            {
                return new O_Forgot()
                {
                    errordata = new errordata()
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
                    errordata = new errordata()
                    {
                        code = 13,
                        Detail = Useful.geterrordata(13)
                    }
                };
            }

            return new O_Forgot()
            {
                id = userdata.user_id,
                retoken = Useful.GetMd5Hash(MD5.Create(), userdata.user_id.ToString() + "reset")
            };
            
            
        }

        [HttpPost]
        [ActionName("Reset")]
        public O_Reset Reset(I_Reset i_data)
        {
            if (i_data == null || string.IsNullOrEmpty(i_data.retoken))
            {
                return new O_Reset()
                {
                    result = false,
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (Useful.VerifyMd5Hash(MD5.Create(), i_data.id.ToString() + "reset", i_data.retoken))
            {
                user edituser = db.user.Where(u => u.user_id == i_data.id).SingleOrDefault();

                edituser.user_password = i_data.newpassword;

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
                    errordata = new errordata()
                    {
                        code = 14,
                        Detail = Useful.geterrordata(14)
                    }
                };
            }
            
        }
    }
}