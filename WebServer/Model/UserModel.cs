namespace WebServer.Model
{
    public class UserModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Birthyear { get; set; }

        public string? BookmarksUrl { get; set; }
        
        //include bookmarks and ratings
    }
}
