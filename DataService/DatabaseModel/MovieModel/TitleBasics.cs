using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleBasics
    {
        public string? TConst { get; set; }
        public string? TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int RuntimeMinutes { get; set; }

        public IList<TitleEpisode>? TitleEpisode { get; set; }
        public TitleRatings? TitleRatings { get; set; }
        public IList<Genres>? Genres { get; set; }
    }
}
