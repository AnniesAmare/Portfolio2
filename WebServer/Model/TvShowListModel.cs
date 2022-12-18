using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class TvShowListModel
    {
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public IList<TvShowModel>? TvShowContentList { get; set; }
        public IList<DirectorListElementModel>? DirectorListWithUrl { get; set; }
        public float? Rating { get; set; }
        public string? Url { get; set; }
    }
}
