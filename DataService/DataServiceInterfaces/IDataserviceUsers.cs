using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServiceInterfaces
{
    public interface IDataserviceUsers
    {
        //USER COMMANDS
        bool UserExists(string username);

        bool CreateUser(string username, string password, string salt, string email, string birthyear);

        User? GetUser(string username);

        bool DeleteUser(string username);

        bool UpdateUser(string username, string email, string birthyear);
    }
}
