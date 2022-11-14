using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class PersonListModel
    {
        public string? Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? Popularity { get; set; }
        public string? PersonUrl { get; set; }

        public IList<TitleListElementModel>? KnownForMoviesWithUrl { get; set; }
        public IList<TitleListElementModel>? KnownForTvShowsWithUrl { get; set; }
    }
}
