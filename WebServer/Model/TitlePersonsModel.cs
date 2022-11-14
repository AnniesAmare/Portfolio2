namespace WebServer.Model
{
    public class TitlePersonsModel
    {
        public string? Title { get; set; }
        public string? TitleUrl { get; set; }

        public IList<TitlePersonListModel> PersonsList { get; set; }
    }
}
