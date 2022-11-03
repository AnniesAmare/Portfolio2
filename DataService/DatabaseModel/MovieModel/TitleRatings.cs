using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleRatings
    {
        public string? TConst { get; set; }
        public TitleBasics? TitleBasics { get; set; }

        //note that this take a dot not comma.
        //Not sure if that work in conversion for the numeric datatype
        public decimal? AverageRating { get; set; } 
        public int? NumVotes { get; set; }

    }
}
