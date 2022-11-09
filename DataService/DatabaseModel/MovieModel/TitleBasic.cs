using DataLayer.DatabaseModel.MovieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleBasic
    {
        public string? TConst { get; set; }
        public string? TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public bool IsTvShow { get; set; }

        public IList<TitleEpisode>? TitleEpisode { get; set; }
        public TitleRating? TitleRating { get; set; }
        public IList<Genre>? Genre { get; set; }
        public OmdbData OmdbData { get; set; } 
        public IList<Wi>? Wi { get; set; }
        public IList<KnownFor>? KnownFor { get; set; }
        public IList<TitleAka>? TitleAka { get; set; }
        public IList<TitlePrincipal>? TitlePrincipal { get; set; }
        public IList<Job>? Job { get; set; }
        public IList<Character>? Character { get; set; }

        //USER FRAMEWORK
        public IList<BookmarkTitle> BookmarkTitle { get; set; }
    }
}
