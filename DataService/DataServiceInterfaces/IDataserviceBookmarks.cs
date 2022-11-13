using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServiceInterfaces
{
    public interface IDataserviceBookmarks
    {
        IList<BookmarkListElement>? GetPersonBookmarks(string username);

        IList<BookmarkListElement>? GetTitleBookmarks(string username);

        IList<BookmarkListElement>? GetBookmarks(string username);
        IList<BookmarkListElement>? GetBookmarks(string username, int page, int pageSize);

        int GetNumberOfBookmarks(string username);

        bool CreateBookmark(string username, string id, string? name);

        bool DeleteBookmark(string username, string id);

        BookmarkElement? NameBookmark(string username, string id, string annotation);
    }
}
