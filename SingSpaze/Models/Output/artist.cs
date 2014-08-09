using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingSpaze.Models.Output
{
    public class O_ArtistList
    {
        public IEnumerable<artistdata> listartist { get; set; }
        public errordata errordata { get; set; }
    }
}