using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class SpecificPersonModel
    {
        public string Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public IList<string> ProfessionList { get; set; }
        public IList<TitleListElementModel> KnownForListWithUrl { get; set; }
        public string? Bookmark { get; set; }
    }

    public class TitleListElementModel
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
    }

}