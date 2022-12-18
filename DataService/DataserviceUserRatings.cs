using DataLayer.DatabaseModel;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataserviceUserRatings : IDataserviceUserRatings
    {

        public IList<UserRatingElement> GetUserRatings(string username, int page, int pageSize)
        {

            //Console.WriteLine("I reach here");
            using var db = new PortfolioDBContext();
            var ratings = db.UserRatings
                .Include(x => x.TitleBasic)
                .Where(x => x.Username == username)
                .Select(x => new UserRatingElement{ 
                Title = x.TitleBasic.PrimaryTitle,
                TConst = x.TConst,
                Rating = x.Rating,
                Date = x.Date
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderBy(x => x.Date)   
                .ToList();        
            
            return ratings;
        }

        public bool InsertUserRating(string username, string id, int rating)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
          
            if (title != null)
            {
                try
                {
                    db.Database.ExecuteSqlInterpolated
                                ($"select user_rate({username},{id},{rating})");
                }
                catch
                {
                    return false;
                }

                var created = db.UserRatings.Find(id);

                if (created != null) return false;
                
                return true;

            }

            return false;
        }

        //var title = db.TitleBasics.Find(id);
        //var person = db.NameBasics.Find(id);

        //    if (title != null)
        //{
        //    var tConst = title.TConst.RemoveSpaces();
        //    if (BookmarkExists(username, tConst))
        //    {
        //        var bookmark = db.BookmarksTitles.Where(x => x.Username == username && x.TConst == tConst).FirstOrDefault();
        //        db.BookmarksTitles.Remove(bookmark);
        //        db.SaveChanges();
        //        return true;
        //    }
        //}

        public bool DeleteUserRatings(string username, string id)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id.RemoveSpaces());

            if (title != null)
            {
                try
                {
                    db.Database.ExecuteSqlInterpolated
                        ($"select delete_rating({username},{title.TConst})");
                }
                catch
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        //helper functions

        public int GetNumberOfUserRatings(string username, int page, int pageSize)
        {
            var allRatings = GetUserRatings(username, page, pageSize);
            var result = allRatings.Count;
            return result;
        }



    }
}
