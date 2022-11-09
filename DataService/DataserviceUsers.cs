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
            return false;
        }

        public User GetUser(string username)
        {
            using var db = new PortfolioDBContext();
            return db.Users.FirstOrDefault(x => x.Username == username);
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

    }
}
