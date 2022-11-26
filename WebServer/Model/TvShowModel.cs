using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class TvShowModel
    {
        public int? Season { get; set; }

        public IList<EpisodeModel>? Episodes { get; set; }
    }
}
