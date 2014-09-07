using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SingSpaze.Models;

namespace SingSpaze.Controllers.API
{
    /// <summary>
    /// Useful api (Data from database)
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

    }
}
