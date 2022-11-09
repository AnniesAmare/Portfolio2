using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DatabaseModel;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;

namespace DataLayer
{
    public class DataserviceBookmarks : IDataserviceBookmarks
    {
        public IList<BookmarkListElement> getPersonBookmarks(string username)
        {
            using var db = new PortfolioDBContext();
            var bookmarks = db.BookmarksNames
                .Where(x => x.Username == username)
                .Select(x => ObjectMapper.Mapper.Map<BookmarkListElement>(x))
                .ToList();
            return bookmarks;
        }

        public IList<BookmarkListElement> getTitleBookmarks(string username)
        {
            using var db = new PortfolioDBContext();
            var bookmarks = db.BookmarksTitles
                .Where(x => x.Username == username)
                .Select(x => ObjectMapper.Mapper.Map<BookmarkListElement>(x))
                .ToList();
            return bookmarks;
        }

        public (IList<BookmarkListElement> titleBookmarks, IList<BookmarkListElement> personBookmarks) getBookmarks(string username)
        {
            var titleBookmarks= getTitleBookmarks(username);
            var personBookmarks = getPersonBookmarks(username);
            return (titleBookmarks, personBookmarks);
        }

        public BookmarkElement createBookmark(string username, string id, string? name)
        {
            using var db = new PortfolioDBContext();
            var tConst = db.TitleBasics.Find(id).TConst;
            var nConst = db.NameBasics.Find(id).NConst;

            if (tConst != null)
            {
                var bookmark = new BookmarkTitle
                {
                    Username = username,
                    TConst = tConst,
                    Annotation = name
                };
                db.BookmarksTitles.Add(bookmark);
                db.SaveChanges();
                return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark);
            }

            if (nConst != null)
            {
                var bookmark = new BookmarkName
                {
                    Username = username,
                    NConst = nConst,
                    Annotation = name
                };
                db.BookmarksNames.Add(bookmark);
                db.SaveChanges();
                return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark);
            }
            return null;
        }

        public BookmarkElement deleteBookmark(string username, string id)
        {
            throw new NotImplementedException();
        }

        public BookmarkElement nameBookmark(string username, string id, string annotation)
        {
            throw new NotImplementedException();
        }
    }
}
