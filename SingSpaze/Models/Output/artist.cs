using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    /// <summary>
    /// Class output data for artistlist
    /// </summary>
    public class O_ArtistList
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class artistdata 
        /// </summary>
        public List<Listartistdata> listartist { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
        
    }

    /// <summary>
    /// Class output data for artistdetails
    /// </summary>
    public class O_ArtistDetails
    {
        /// <summary>
        /// Class Artistdata 
        /// </summary>
        public Artistdata artistdata { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }

    /// <summary>
    /// Class output for search artist (ex.resultNumber ,listartist)
    /// </summary>
    public class O_SearchArtist
    {
        /// <summary>
        /// Number of this result
        /// </summary>
        public int resultNumber { get; set; }
        /// <summary>
        /// List class listartistdata
        /// </summary>
        public List<Artistdata> listartist { get; set; }

        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
}