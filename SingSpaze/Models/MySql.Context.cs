﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SingSpaze.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class singspazeEntities : DbContext
    {
        public singspazeEntities()
            : base("name=singspazeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<accessrule> accessrule { get; set; }
        public DbSet<album> album { get; set; }
        public DbSet<artist> artist { get; set; }
        public DbSet<contactus> contactus { get; set; }
        public DbSet<csv> csv { get; set; }
        public DbSet<deviceinfo> deviceinfo { get; set; }
        public DbSet<genre> genre { get; set; }
        public DbSet<language> language { get; set; }
        public DbSet<myrecord> myrecord { get; set; }
        public DbSet<playlist> playlist { get; set; }
        public DbSet<playlisttosong> playlisttosong { get; set; }
        public DbSet<publisherforartist> publisherforartist { get; set; }
        public DbSet<publisherforsong> publisherforsong { get; set; }
        public DbSet<singinghistory> singinghistory { get; set; }
        public DbSet<song> song { get; set; }
        public DbSet<songrequest> songrequest { get; set; }
        public DbSet<splashpage> splashpage { get; set; }
        public DbSet<status> status { get; set; }
        public DbSet<user> user { get; set; }
        public DbSet<viewhistory> viewhistory { get; set; }
        public DbSet<wtbtokens> wtbtokens { get; set; }
        public DbSet<accessruletocountrycode> accessruletocountrycode { get; set; }
    }
}
