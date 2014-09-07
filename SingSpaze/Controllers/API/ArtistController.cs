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
    /// <summary>
    /// Artist api
    /// </summary>
    public class ArtistController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        /// <summary>
        /// Send data to get list artist
        /// </summary>
        /// <param name="i_data">Class I_ArtistList</param>
        /// <returns>Class O_ArtistList</returns>
        [HttpPost]
        [ActionName("List")]
        public O_ArtistList List(I_ArtistList i_data)
        {
            if (i_data == null)
            {
                return new O_ArtistList()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            List<artist> listartist = new List<artist>();

            var before = DateTime.Now.AddDays(-i_data.time);

            //song_id to artist_id
            var viewhistory = (from view in db.viewhistory
                               join song in db.song
                               on view.Song_Id equals song.song_id
                               join artist in db.artist
                               on song.song_artistId equals artist.artist_id
                               select new { artist_id = artist.artist_id,date = view.ViewHistory_Date }).ToList();
            //group and count artist_id
            var groupartist = from history in viewhistory
                              where history.date > before
                              group history by history.artist_id into ghistory
                              select new { artist_id = ghistory.Key, count = ghistory.Count() };
            // order
            if (string.IsNullOrEmpty(i_data.type))
                listartist = db.artist.OrderBy(s => s.artist_description_th).ToList();
            //else if (i_data.type == "new")
            //    listartist = db.artist.Where(s => s.song_releasedDate ?? DateTime.Now > before).OrderByDescending(s => s.song_releasedDate).ToList();
            else if (i_data.type == "hot")
            {
                //var groupartist = from history in db.singinghistory
                //                       where history.singinghistory_date > before
                //                       group history by history.artist_id into ghistory
                //                       select new { artist_id = ghistory.Key, count = ghistory.Count() };

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
            if (i_data.artist_type != null)
                listartist = listartist.Where(a => a.artist_type == i_data.artist_type).ToList();

            int resultNumber = listartist.Count();

            // skip take
            listartist = listartist.Skip(i_data.selectdata.startindex-1).Take(i_data.selectdata.endindex-i_data.selectdata.startindex+1).ToList();

            if (listartist == null)
            {
                return new O_ArtistList()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<Listartistdata> o_listartist = new List<Listartistdata>();

            foreach (artist data in listartist)
            {
                int view = Useful.getview(data.artist_id,"artist"); // alltime
                if(i_data.type == "hot")
                    view = groupartist.Where(s => s.artist_id == data.artist_id).Select(s => s.count).SingleOrDefault();

                o_listartist.Add(new Listartistdata()
                {
                    id = data.artist_id,
                    description_TH = data.artist_description_th,
                    description_EN = data.artist_description_en,
                    picture = data.artist_picture,
                    artistType = data.artist_type,
                    view = view
                });
            }
            
           
            return new O_ArtistList()
            {
                resultNumber = resultNumber,
                listartist = o_listartist
            };
        }

        /// <summary>
        /// Send artist id to get artist detail
        /// </summary>
        /// <param name="i_data"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Details")]
        public O_ArtistDetails Details(I_ArtistDetails i_data)
        {
            if (i_data == null)
            {
                return new O_ArtistDetails()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            artist data = db.artist.FirstOrDefault(a => a.artist_id == i_data.id);

            if (data == null)
            {
                return new O_ArtistDetails()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            return new O_ArtistDetails()
            {
                artistdata = new Artistdata()
                {
                    id = data.artist_id,
                    description_TH = data.artist_description_th,
                    description_EN = data.artist_description_en,
                    artistType = data.artist_type,
                    picture = data.artist_picture,
                    songs = db.song.Where( s => s.song_artistId == data.artist_id).Count()
                }
            };
        }
    }
}
