using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class TvShowListModel
    {
        public string? TConst { get; set; }
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public IList<TvShowListElement>? TvShowContentList { get; set; }
        public float? Rating { get; set; }
        public string? Url { get; set; }
    }
}
