using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;

namespace DataLayer
{
    public class DataserviceBookmarks : IDataserviceBookmarks
    {
        public IList<BookmarkListElement> getPersonBookmarks(string username)
        {
            throw new NotImplementedException();
        }

        public IList<BookmarkListElement> getTitleBookmarks(string username)
        {
            throw new NotImplementedException();
        }

        public (IList<BookmarkListElement> titleBookmarks, IList<BookmarkListElement> personBookmarks) getBookmarks(string username)
        {
            throw new NotImplementedException();
        }

        public BookmarkElement createBookmark(string id)
        {
            throw new NotImplementedException();
        }

        public BookmarkElement deleteBookmark(string id)
        {
            throw new NotImplementedException();
        }

        public BookmarkElement nameBookmark(string id, string annotation)
        {
            throw new NotImplementedException();
        }
    }
}
