using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleRating
    {
        public string? TConst { get; set; }
        public TitleBasic? TitleBasic { get; set; }

        //note that this take a dot not comma.
        //Not sure if that work in conversion for the numeric datatype
        public float? AverageRating { get; set; } 
        public int? NumVotes { get; set; }

    }
}
