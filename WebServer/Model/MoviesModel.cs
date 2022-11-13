using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class MoviesModel
    {
        public string? TConst { get; set; }
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public IList<DirectorListElement>? DirectorList { get; set; }
        public float? Rating { get; set; }
        public string? Url { get; set; }
    }
}
