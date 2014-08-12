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
        /// List class artistdata 
        /// </summary>
        public List<Artistdata> listartist { get; set; }
        /// <summary>
        /// Class errordata
        /// </summary>
        public Errordata errordata { get; set; }
    }
}