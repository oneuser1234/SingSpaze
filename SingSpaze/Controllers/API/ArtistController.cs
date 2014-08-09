using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SingSpaze.Models.Output;
using SingSpaze.Models.Input;
using SingSpaze.Models;

namespace SingSpaze.Controllers.API
{
    public class ArtistController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        [HttpPost]
        [ActionName("List")]
        public O_ArtistList ArtistList(I_ArtistList i_data)
        {
            if (i_data == null)
            {
                return new O_ArtistList()
                {
                    errordata = new errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            List<artist> listartist = new List<artist>();

            var before = DateTime.Now.AddDays(-i_data.time);

            // order
            if (string.IsNullOrEmpty(i_data.type))
                listartist = db.artist.OrderBy(s => s.artist_id).ToList();
            //else if (i_data.type == "new")
            //    listartist = db.artist.Where(s => s.song_releasedDate > before).OrderByDescending(s => s.song_releasedDate).ToList();
            else if (i_data.type == "hot")
            {
                var groupartist = from history in db.singinghistory
                                       where history.singinghistory_date > before
                                       group history by history.artist_id into ghistory
                                       select new { artist_id = ghistory.Key, count = ghistory.Count() };

                var joinhistory = from dbartist in db.artist.AsEnumerable()
                                  join history in groupartist.AsEnumerable()
                                  on dbartist.artist_id equals history.artist_id
                                  orderby history.count descending
                                  select dbartist;
                //select new { dbsong,count = history.count}

                listartist = joinhistory.ToList();

            }
            //else if (i_data.type == "recommend")
            //    listartist = db.artist.ToList();

            // where
            if(i_data.artist_id != null)
                listartist = listartist.Where(a => Useful.getlistdata(i_data.artist_id).Contains(a.artist_id)).ToList();

            // skip take
            listartist = listartist.Skip(i_data.selectdata.skip).Take(i_data.selectdata.take).ToList();

            if (listartist == null)
            {
                return new O_ArtistList()
                {
                    errordata = new errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<artistdata> o_artist = new List<artistdata>();

            foreach (artist data in listartist)
            {
                o_artist.Add(new artistdata()
                {
                    id = data.artist_id,
                    description_TH = data.artist_description_th,
                    description_EN = data.artist_description_en
                });
            }

            return new O_ArtistList()
            {
                listartist = o_artist
            };
        }
    }
}
