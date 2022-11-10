using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class TvShowListElement
    {
        public int Season { get; set; }
        public IList<EpisodeListElement> Episodes { get; set; }
    }
}
