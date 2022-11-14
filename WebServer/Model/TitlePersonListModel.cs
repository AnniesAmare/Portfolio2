using DataLayer.DataTransferModel;

namespace WebServer.Model
{
    public class TitlePersonListModel
    {
        public string? Name { get; set; }
        public string ProductionRole { get; set; }
        public IList<CharacterListElement> CharacterList { get; set; }
        public float? Popularity { get; set; }
        public string? PersonUrl { get; set; }
    }
}
