using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class TitlePersons
    {
        public string? NConst { get; set; }
        public string? Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? Popularity { get; set; }
        public bool isActor { get; set; }
        public bool isMovie { get; set; }
        public bool isTvShow { get; set; }

        public IList<TitleListElement>? KnownForMovies { get; set; }
        public IList<TitleListElement>? KnownForTvShows { get; set; }
    }
}
