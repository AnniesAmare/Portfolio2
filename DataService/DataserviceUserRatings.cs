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

namespace DataLayer
{
    public class DataserviceUserRatings : IDataserviceUserRatings
    {

        public bool InsertUserRating(string username, string id, int rating)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.Find(id);
            //var user = db.UserRatings.Find(username);

            Console.WriteLine(username);
            Console.WriteLine(id);
            Console.WriteLine(rating);

            try
            {
                //add user search
                var created = db.Database.ExecuteSqlInterpolated
                    ($"select * save_user_rating({username},{id},{rating})");

                if(created > 0)
                {
                    return true;
                }

                return false;
                
            }catch{
                return false;
            }

            

            //var searchResult = 
            //    db.PersonSearches.FromSqlInterpolated($"select * from string_search_name({search})").ToList();

            //if (title != null)
            //{
            //    Console.WriteLine("got hereTest");
            //    //var userRatingTitle =
            //    //    db.UserRatings.ToList();
            //        //.Where(x => x.Username == username)
            //        //.FirstOrDefault();

            //    //if (userRatingTitle == null) userRatingTitle. = "Not here";
            //    //Console.WriteLine(userRatingTitle  + "heres");
            //    var tConst = title.TConst.RemoveSpaces();
            //    var userName = username.RemoveSpaces();

            //    //Console.WriteLine(UserRatingExists(username, id));
            //    //if (!UserRatingExists(username, id))
            //    //{
            //        Console.WriteLine("got here2");
            //        var newRating = CreateUserRating(username, id, rating);
            //    Console.WriteLine(newRating.Username);
            //    Console.WriteLine(newRating.Rating);
            //    //Console.WriteLine(newRating.Date);
            //    //Console.WriteLine(newRating.Title);



            //   // var userRating = ObjectMapper.Mapper.Map<UserRatingElement>(newRating);

            //    //Console.WriteLine(userRating.Username + "test");
            //    //Console.WriteLine(userRating);


            //    //db.UserRatings.Add(newRating);

            //    db?.UserRatings?.Add(newRating);
            //    //db?.UserRatings?.AddObject(userRating);
            //    Console.WriteLine(newRating + "arrived");
            //    db.SaveChanges();
            //    Console.WriteLine("got her3");

            //        return true;
            //    //}
            //}

            //return false;
        }

        //helper functions
        public UserRating CreateUserRating(string username, string tConst, int rating)
        {
            var newRating = new UserRating
            {
                Username = username,
                TConst = tConst.RemoveSpaces(),
                Rating = rating,
                //Date = DateTime.Now,

            };
            return newRating;
        }


        private bool UserRatingExists(string username, string id)
        {
            using var db = new PortfolioDBContext();
            var userRatingTitle = db.UserRatings.Where(x => x.Username == username && x.TConst == id)!.FirstOrDefault();
            Console.WriteLine(userRatingTitle);
            if (userRatingTitle == null) return false;
            return true;
        }

    }
}
