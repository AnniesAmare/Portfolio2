using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class Persons
    {
        public string? NConst { get; set; }
        public string? Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? Popularity { get; set; }
        //public string? Profession { get; set; }
        public Boolean isActor { get; set; }

        public List<TitleListElement>? KnowsForMovies { get; set; }
        public List<TitleListElement>? KnowsForTvShows { get; set; }

    }
}


//name, popularity, movies / Tv-shows (titles) 