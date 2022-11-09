using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? BirthYear { get; set; }
        public string? Email { get; set; }

        public IList<BookmarkName>? BookmarkName { get; set; }
        public IList<BookmarkTitle>? BookmarkTitle { get; set; }
        public IList<UserRating>? UserRating { get; set; }
        public IList<UserSearch>? UserSearch { get; set; }
        public IList<Review>? Review { get; set; }
    }
}
