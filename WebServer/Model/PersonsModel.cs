using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class PersonsModel
    {
        public string? NConst { get; set; }
        public string? Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? Popularity { get; set; }

        public List<Titles>? KnowsForMovies { get; set; }
        public List<Titles>? KnowsForTvShows { get; set; }
    }
}
