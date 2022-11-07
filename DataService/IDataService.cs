using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasic> GetTitleBasics();
        //IList<NameBasics> GetNameBasics();

        //SPECIFIC TITLE METHODS
        SpecificTitle GetSpecificTitleByName(string name);
        SpecificTitle GetSpecificTitle(string tConst);

        //SPECIFIC PERSON METHODS
        SpecificPerson GetSpecificPersonByName(string name);
        SpecificPerson GetSpecificPerson(string nConst);

        //USER COMMANDS
        bool UserExists(string username);

        User CreateUser(string username, string password, string salt, string email, string birthyear);

        User GetUser(string username);

        /*
        IList<Category> GetCategories();
        Category? GetCategory(int id);
        Category CreateCategory(string name, string description);
        bool UpdateCategory(int id, string name, string description);
        bool DeleteCategory(int id); 
        */
    }
}
