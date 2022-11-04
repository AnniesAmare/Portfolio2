using DataLayer.DatabaseModel.MovieModel;
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
        public int? RuntimeMinutes { get; set; }

        public IList<TitleEpisode>? TitleEpisode { get; set; }
        public TitleRatings? TitleRatings { get; set; }
        public IList<Genres>? Genres { get; set; }
        public OmdbData OmdbData { get; set; } 
        public IList<Wi>? Wi { get; set; }
        public IList<KnownFor>? KnownFor { get; set; }
        public IList<TitleAkas>? TitleAkas { get; set; }
        public IList<TitlePrincipals>? TitlePrincipals { get; set; }
        public IList<Jobs>? Jobs { get; set; }
        public IList<Characters>? Characters { get; set; }
    }
}
