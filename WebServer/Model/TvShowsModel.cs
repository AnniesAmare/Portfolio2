using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class TvShowsModel
    {
        public string? TConst { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public IList<DirectorListElement>? DirectorList { get; set; }
        public IList<TvShowListElement>? TvShowContentList { get; set; }
        public float? Rating { get; set; }
    }
}
