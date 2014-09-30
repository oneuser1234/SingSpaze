using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SingSpaze.Models.Input
{
    /// <summary>
    /// Class input data for register
    /// </summary>
    [DataContract]
    public class I_Register
    {
        /// <summary>
        /// Username 
        /// </summary>
        //[DataMember(Name="username")]
        //public string Username { get; set; }
        /// <summary>
        /// Password 
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
        /// <summary>
        /// Firstname
        /// </summary>
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        /// <summary>
        /// Lastname
        /// </summary>
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        /// <summary>
        /// Email 
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }
        
    }

    [DataContract]
    public class I_FBRegister
    {
        [DataMember(Name = "fbuserid")]
        public int fbUserId { get; set; }
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }

        
    }
    /// <summary>
    /// Class input data for login
    /// </summary>
    [DataContract]
    public class I_Login
    {
        /// <summary>
        /// Username
        /// </summary>
        //[DataMember(Name = "username")]
        //public string Username { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "email")]
        public string email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [DataMember(Name = "password")]
        public string password { get; set; }
        
        /// <summary>
        /// Mac address 
        /// </summary>
        [DataMember(Name = "mac_address")]
        public string Mac_Address { get; set; }
        /// <summary>
        /// Device ID
        /// </summary>
        [DataMember(Name = "device_id")]
        public string Device_ID { get; set; }
        
       
    }

    /// <summary>
    /// Class input data for Fblogin
    /// </summary>
    [DataContract]
    public class I_FBLogin
    {
        /// <summary>
        /// Facebook Id
        /// </summary>
        [DataMember(Name = "fbuserid")]
        public int fbUserId { get; set; }
        /// <summary>
        /// Firstname 
        /// </summary>
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        /// <summary>
        /// Lastname 
        /// </summary>
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        /// <summary>
        /// Email 
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }
        /// <summary>
        /// Mac address 
        /// </summary>
        [DataMember(Name = "mac_address")]
        public string Mac_Address { get; set; }
        /// <summary>
        /// Device ID
        /// </summary>
        [DataMember(Name = "device_id")]
        public string Device_ID { get; set; }

    }

    /// <summary>
    /// Class input data for profile
    /// </summary>
    [DataContract]
    public class I_Profile
    {
        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }        
    }
    /// <summary>
    /// Class input data for editprofile
    /// </summary>
    [DataContract]
    public class I_EditProfile
    {
        /// <summary>
        /// Old password
        /// </summary>
        [DataMember(Name = "oldpassword")]
        public string oldPassword { get; set; }
        /// <summary>
        /// New password
        /// </summary>
        [DataMember(Name = "newpassword")]
        public string newPassword { get; set; }
        /// <summary>
        /// Firstname
        /// </summary>
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }
        /// <summary>
        /// Lastname
        /// </summary>
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Class logindata
        /// </summary>
        [DataMember(Name = "logindata")]
        public Logindata logindata { get; set; }
    }
    /// <summary>
    /// Class input data for forgot
    /// </summary>
    [DataContract]
    public class I_Forgot 
    {
        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "email")]
        public string email { get; set; }
    }
    /// <summary>
    /// Class input data for reset
    /// </summary>
    [DataContract]
    public class I_Reset
    {
        /// <summary>
        /// Reset token get from Forgot
        /// </summary>
        [DataMember(Name = "retoken")]
        public string retoken { get; set; }
        /// <summary>
        /// New password
        /// </summary>
        [DataMember(Name = "newpassword")]
        public string newpassword { get; set; }

    }

    

}