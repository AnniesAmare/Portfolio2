using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public bool createBookmark(string username, string id, string? name)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
            var person = db.NameBasics.Find(id);

            name = string.IsNullOrEmpty(name) ? "Unnamed" : name;

            if (title != null)
            {
                var tConst = title.TConst;
                if (!BookmarkExists(username, tConst))
                {
                    CreateTitleBookmark(username, tConst, name);
                    return true;
                }
            }

            if (person != null)
            {
                var nConst = person.NConst;
                if (!BookmarkExists(username, nConst))
                {
                    CreatePersonBookmark(username, nConst, name);
                    return true;
                }
            }
            return false;
        }

        public bool deleteBookmark(string username, string id)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
            var person = db.NameBasics.Find(id);

            if (title != null)
            {
                var tConst = title.TConst.RemoveSpaces();
                if (BookmarkExists(username, tConst))
                {
                    var bookmark = db.BookmarksTitles.Where(x => x.Username == username && x.TConst == tConst).FirstOrDefault();
                    Console.WriteLine(bookmark.Username, bookmark.TConst);
                    db.BookmarksTitles.Remove(bookmark);
                    db.SaveChanges();
                    return true;
                }
            }

            if (person != null)
            {
                var nConst = person.NConst.RemoveSpaces();
                if (BookmarkExists(username, nConst))
                {
                    var bookmark = db.BookmarksNames.Where(x => x.Username == username && x.NConst == nConst).FirstOrDefault();
                    db.BookmarksNames.Remove(bookmark);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public BookmarkElement nameBookmark(string username, string id, string annotation)
        {
            throw new NotImplementedException();
        }

        //helper functions
        private bool BookmarkExists(string username, string id)
        {
            using var db = new PortfolioDBContext();
            var bookmarkName = db.BookmarksNames.Where(x => x.Username == username && x.NConst == id)!.FirstOrDefault();
            var bookmarkTitle = db.BookmarksTitles.Where(x => x.Username == username && x.TConst == id)!.FirstOrDefault();
            if (bookmarkTitle == null && bookmarkName == null) return false;
            return true;
        }

        private BookmarkElement CreateTitleBookmark(string username, string tConst, string? name)
        {
            using var db = new PortfolioDBContext();
            var bookmark = new BookmarkTitle
            {
                Username = username,
                TConst = tConst.RemoveSpaces(),
                Annotation = name
            };
            db.BookmarksTitles.Add(bookmark);
            db.SaveChanges();
            return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark);
        }

        private BookmarkElement CreatePersonBookmark(string username, string nConst, string? name)
        {
            using var db = new PortfolioDBContext();
            var bookmark = new BookmarkName
            {
                Username = username,
                NConst = nConst.RemoveSpaces(),
                Annotation = name
            };
            db.BookmarksNames.Add(bookmark);
            db.SaveChanges();
            return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark);
        }

    }
}
