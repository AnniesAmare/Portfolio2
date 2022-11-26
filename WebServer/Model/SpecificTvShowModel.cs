using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class SpecificTvShowModel
    {
        public string? TConst { get; set; }
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public IList<DirectorListElement>? DirectorList { get; set; }
        public IList<TvShowListElement>? TvShowContentList { get; set; }

        //new elements
        public IList<DirectorListElementModel>? DirectorListWithUrl { get; set; }
        public IList<TvShowModel>? TvShowContent { get; set; }
        public float? Rating { get; set; }
    }
}
