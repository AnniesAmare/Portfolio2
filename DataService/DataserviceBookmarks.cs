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
        public IList<BookmarkListElement> GetPersonBookmarks(string username)
        {
            using var db = new PortfolioDBContext();
            var bookmarks = db.BookmarksNames
                .Where(x => x.Username == username)
                .Select(x => ObjectMapper.Mapper.Map<BookmarkListElement>(x))
                .ToList();
            return bookmarks;
        }

        public IList<BookmarkListElement> GetTitleBookmarks(string username)
        {
            using var db = new PortfolioDBContext();
            var bookmarks = db.BookmarksTitles
                .Where(x => x.Username == username)
                .Select(x => ObjectMapper.Mapper.Map<BookmarkListElement>(x))
                .ToList();
            return bookmarks;
        }

        public IList<BookmarkListElement> GetBookmarks(string username)
        {
            var titleBookmarks= GetTitleBookmarks(username);
            var personBookmarks = GetPersonBookmarks(username);
            if (titleBookmarks == null && personBookmarks == null) return null;

            if (personBookmarks != null)
            {
                foreach (var personBookmark in personBookmarks)
                {
                    personBookmark.isPerson = true;
                    personBookmark.isTitle = false;
                }
            }
            if (titleBookmarks != null)
            {
                foreach (var titleBookmark in titleBookmarks)
                {
                    titleBookmark.isPerson = false;
                    titleBookmark.isTitle = true;
                }
            }

            if ( personBookmarks == null) return titleBookmarks;
            if (titleBookmarks == null) return personBookmarks;

            var allBookmarks = titleBookmarks.Concat(personBookmarks).ToList();
            return allBookmarks;
        }

        public IList<BookmarkListElement> GetBookmarks(string username, int page, int pageSize)
        {
            var allBookmarks = GetBookmarks(username);
            var pagedBookmarks = allBookmarks
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedBookmarks;
        }

        public int GetNumberOfBookmarks(string username)
        {
            var allBookmarks = GetBookmarks(username);
            var result = allBookmarks.Count;
            return result;
        }





        public bool CreateBookmark(string username, string id, string? name)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
            var person = db.NameBasics.Find(id);

            name = string.IsNullOrEmpty(name) ? "Unnamed" : name;

            if (title != null)
            {
                var tConst = title.TConst.RemoveSpaces();
                if (!BookmarkExists(username, tConst))
                {
                    var bookmark = CreateTitleBookmark(username, tConst, name);
                    db.BookmarksTitles.Add(bookmark);
                    db.SaveChanges();
                    return true;
                }
            }

            if (person != null)
            {
                var nConst = person.NConst.RemoveSpaces();
                if (!BookmarkExists(username, nConst))
                {
                    var bookmark = CreatePersonBookmark(username, nConst, name);
                    db.BookmarksNames.Add(bookmark);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteBookmark(string username, string id)
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

        public BookmarkElement NameBookmark(string username, string id, string annotation)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
            var person = db.NameBasics.Find(id);

            if (title != null)
            {
                var tConst = title.TConst;
                if (BookmarkExists(username, tConst))
                {
                    var bookmark = db.BookmarksTitles.Find(username, tConst);
                    bookmark.Annotation = annotation;
                    db.SaveChanges();
                    return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark); ;
                }
            }

            if (person != null)
            {
                var nConst = person.NConst.RemoveSpaces();
                if (BookmarkExists(username, nConst))
                {
                    var bookmark = db.BookmarksNames.Find(username, nConst);
                    bookmark.Annotation = annotation;
                    db.SaveChanges();
                    return ObjectMapper.Mapper.Map<BookmarkElement>(bookmark);
                }
            }

            return null;
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

        private BookmarkTitle CreateTitleBookmark(string username, string tConst, string? name)
        {
            var bookmark = new BookmarkTitle
            {
                Username = username,
                TConst = tConst.RemoveSpaces(),
                Annotation = name
            };
            return bookmark;
        }

        private BookmarkName CreatePersonBookmark(string username, string nConst, string? name)
        {
            var bookmark = new BookmarkName
            {
                Username = username,
                NConst = nConst.RemoveSpaces(),
                Annotation = name
            };
            return bookmark;
        }

    }
}
