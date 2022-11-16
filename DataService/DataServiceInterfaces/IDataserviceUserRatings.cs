using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServiceInterfaces
{
    public interface IDataserviceUserRatings
    {
        IList<UserRatingElement> GetUserRatings(string username, int page, int pageSize);
        bool InsertUserRating(string username,string id, int rating);
        int GetNumberOfUserRatings(string username,int page, int pageSize);
        bool DeleteUserRatings(string username, string id);
    }
}
