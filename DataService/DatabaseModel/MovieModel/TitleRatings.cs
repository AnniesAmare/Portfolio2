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
        public string? AverageRating { get; set; }
        public string? NumVotes { get; set; }

    }
}
