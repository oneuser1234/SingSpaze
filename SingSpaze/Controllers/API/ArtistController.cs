using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SingSpaze.Models.Output;
using SingSpaze.Models.Input;
using SingSpaze.Models;
using System.Text;

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

            List<artist> listartist = db.artist.ToList();

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
                listartist = listartist.OrderBy(s => Encoding.GetEncoding("TIS-620").GetString(Encoding.Default.GetBytes(s.artist_description_th))).ToList();
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
            else
                listartist = listartist.OrderBy(s => Encoding.GetEncoding("tis-620").GetString(Encoding.Default.GetBytes(s.artist_description_th))).ToList();

            // where
            if(i_data.artist_id != null)
                listartist = listartist.Where(a => Useful.getlistdata(i_data.artist_id).Contains(a.artist_id)).ToList();
            if (i_data.artist_type != null)
                listartist = listartist.Where(a => a.artist_type.ToLower() == i_data.artist_type.ToLower()).ToList();

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

            List<Artistdata> o_listartist = new List<Artistdata>();

            foreach (artist data in listartist)
            {
                int view = Useful.getview(data.artist_id,"artist"); // alltime
                if(i_data.type == "hot")
                    view = groupartist.Where(s => s.artist_id == data.artist_id).Select(s => s.count).SingleOrDefault();

                //o_listartist.Add(new Listartistdata()
                //{
                //    id = data.artist_id,
                //    description_TH = data.artist_description_th,
                //    description_EN = data.artist_description_en,
                //    picture = data.artist_picture,
                //    artistType = data.artist_type,
                //    view = view,
                //    publisherdata = Useful.getpublisherartistdata(data.artist_publisherforartistId)
                //});

                o_listartist.Add(Useful.getartistdata(int.Parse(data.artist_id.ToString())));
                
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
        /// <param name="i_data">Class I_ArtistDetails</param>
        /// <returns>Class O_ArtistDetails</returns>
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
                artistdata = Useful.getartistdata(int.Parse(data.artist_id.ToString()))
            };
        }


        /// <summary>
        /// For search artist with keyword
        /// </summary>
        /// <param name="i_data">Class I_SearchSong</param>
        /// <returns>Class O_SearchSong</returns>
        [HttpPost]
        [ActionName("SearchArtist")]
        public O_SearchArtist SearchArtist(I_SearchArtist i_data)
        {
            if (i_data == null)
            {
                return new O_SearchArtist()
                {
                    errordata = new Errordata()
                    {
                        code = 11,
                        Detail = Useful.geterrordata(11)
                    }
                };
            }

            if (i_data.keyword == null)
                i_data.keyword = "";

            List<artist> listartist = db.artist.ToList();

            //type
            if (i_data.type != null)
                listartist = listartist.Where(a => a.artist_type.ToLower() == i_data.type.ToLower()).ToList();

            //keyword
            listartist = listartist.Where(a => a.artist_description_th.ToLower().Contains(i_data.keyword.ToLower()) || a.artist_description_en.ToLower().Contains(i_data.keyword.ToLower())).ToList();

            int resultNumber = listartist.Count();

            // skip take
            listartist = listartist.OrderBy(a => Encoding.GetEncoding("TIS-620").GetString(Encoding.Default.GetBytes(a.artist_description_th))).Skip(i_data.selectdata.startindex - 1).Take(i_data.selectdata.endindex - i_data.selectdata.startindex + 1).ToList();

            if (listartist == null)
            {
                return new O_SearchArtist()
                {
                    errordata = new Errordata()
                    {
                        code = 6,
                        Detail = Useful.geterrordata(6)
                    }
                };
            }

            List<Artistdata> o_artist= new List<Artistdata>();


            foreach (artist data in listartist)
            {
                //o_artist.Add(new Artistdata()
                //{
                //    id = data.artist_id,
                //    description_TH = data.artist_description_th,
                //    description_EN = data.artist_description_en,
                //    artistType = data.artist_type,
                //    picture = data.artist_picture,
                //    songs = db.song.Where( s => s.song_artistId == data.artist_id).Count(),
                //    publisherdata = Useful.getpublisherartistdata(data.artist_publisherforartistId)
                //});
                o_artist.Add(Useful.getartistdata(int.Parse(data.artist_id.ToString())));
                
            }

            return new O_SearchArtist()
            {
                listartist = o_artist,
                resultNumber = resultNumber
            };
        }
    }
}
