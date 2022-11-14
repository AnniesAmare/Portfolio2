using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DatabaseModel;
using DataLayer.DataServiceInterfaces;

namespace DataLayer
{
    public class DataserviceUsers : IDataserviceUsers
    {
        //USER COMMANDS
        public bool UserExists(string username)
        {
            using var db = new PortfolioDBContext();
            if (GetUser(username) == null) return false;
            return true;
        }

        public User GetUser(string username)
        {
            using var db = new PortfolioDBContext();
            var user = db.Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        public User CreateUser(string username, string password, string salt, string email, string birthyear)
        {
            using var db = new PortfolioDBContext();
            var user = new User
            {
                Username = username,
                Password = password,
                Salt = salt,
                Email = email,
                BirthYear = birthyear
            };
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public bool UpdateUser(string username, string email, string birthyear)
        {
            using var db = new PortfolioDBContext();
            var user = db.Users.Find(username);
            if (user != null)
            {
                if (!email.IsNullOrEmpty()) user.Email = email;
                if (!birthyear.IsNullOrEmpty()) user.BirthYear = birthyear;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteUser(string username)
        {
            using var db = new PortfolioDBContext();
            var user = GetUser(username);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            } 
            return false;
        }
    }
}
