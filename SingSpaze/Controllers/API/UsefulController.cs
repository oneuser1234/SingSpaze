using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SingSpaze.Models;

namespace SingSpaze.Controllers.API
{
    public class UsefulController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        [HttpGet]
        [ActionName("Language")]
        public IEnumerable<languagedata> Language()
        {
            List<languagedata> response = new List<languagedata>();
            List<language> listlanguagedata = db.language.ToList();

            foreach(language data in listlanguagedata)
            {
                languagedata o_languagedata =  new languagedata()
                {
                    id = data.language_id,
                    description = data.language_description
                };

                response.Add(o_languagedata);
            }

            return response;            
        }

        [HttpGet]
        [ActionName("Genre")]
        public IEnumerable<genredata> Genre()
        {
            List<genredata> response = new List<genredata>();
            List<genre> listgenredata = db.genre.ToList();

            foreach (genre data in listgenredata)
            {
                genredata o_genredata = new genredata()
                {
                    id = data.genre_id,
                    description = data.genre_description
                };

                response.Add(o_genredata);
            }

            return response;
        }

        [HttpGet]
        [ActionName("Banner")]
        public IEnumerable<bannerdata> Banner()
        {
            List<bannerdata> response = new List<bannerdata>();
            //no data
            bannerdata testdata = new bannerdata()
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
