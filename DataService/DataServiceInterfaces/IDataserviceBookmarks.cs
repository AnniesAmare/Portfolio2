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
        IList<BookmarkListElement> getPersonBookmarks(string username);

        IList<BookmarkListElement> getTitleBookmarks(string username);
        
        (IList<BookmarkListElement> titleBookmarks, IList<BookmarkListElement> personBookmarks) getBookmarks(string username);

        bool createBookmark(string username, string id, string? name);

        bool deleteBookmark(string username, string id);

        BookmarkElement nameBookmark(string username, string id, string annotation);
    }
}
