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

        BookmarkElement createBookmark(string id);

        BookmarkElement deleteBookmark(string id);

        BookmarkElement nameBookmark(string id, string annotation);
    }
}
