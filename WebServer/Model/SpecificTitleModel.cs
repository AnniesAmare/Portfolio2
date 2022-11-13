namespace WebServer.Model
{
    public class SpecificTitleModel
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public IList<string>? Genre { get; set; }
        public int? Runtime { get; set; }
        public IList<DirectorListElementModel>? DirectorListWithUrl { get; set; }
        public IList<ActorListElementModel>? ActorListWithUrl { get; set; }
        public float? Rating { get; set; }
        public string? Bookmark { get; set; }

    }

    public class DirectorListElementModel
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
    }

    public class ActorListElementModel
    {
        public string? Name { get; set; }
        public string? Character { get; set; }
        public string? Url { get; set; }

    }


}

