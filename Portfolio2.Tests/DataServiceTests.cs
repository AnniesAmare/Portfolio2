using DataLayer.DataTransferModel;
using System.Linq;

namespace Portfolio2.Tests
{
    public class DataServiceTests
    {


        //Testing whether our Domain Object Model (DOM) methods return expected values

        [Fact]
        public void GetSpecificTitleReturnsCompleteRecordOnValidTConst()
        {
            var service = new DataserviceSpecificTitle();
            var specificTitle = service.GetSpecificTitle("tt0052520");
            Assert.Equal("The Twilight Zone", specificTitle.Title);
            Assert.Equal(53, specificTitle.DirectorList.Count);
            Assert.Contains("Abner Biberman", specificTitle.DirectorList.First().Name);
            Assert.Equal(30, specificTitle.ActorList.Count);
            Assert.Contains("Burgess Meredith", specificTitle.ActorList.First().Name);
            Assert.Contains("Horror", specificTitle.Genre.Last());
        }

        [Fact]
        public void GetSpecificTitleReturnsCompleteRecordOnValidName()
        {
            var service = new DataserviceSpecificTitle();
            var specificTitle = service.GetSpecificTitleByName("Latchkey");
            Assert.Contains("tt0401571", specificTitle.TConst);
            Assert.Equal(1, specificTitle.DirectorList.Count);
            Assert.Contains("Sean Olson", specificTitle.DirectorList.First().Name);
            Assert.Equal(4, specificTitle.ActorList.Count);
            Assert.Contains("Andrew Michaelson", specificTitle.ActorList.First().Name);
            Assert.Contains("Thriller", specificTitle.Genre.Last());
        }



        [Fact]
        public void GetSpecificPersonReturnsCompleteRecordOnValidNConst()
        {
            var service = new DataserviceSpecificPerson();
            var specificPerson = service.GetSpecificPerson("nm0424060");
            Assert.Equal("Scarlett Johansson", specificPerson.Name);
            Assert.Equal(3, specificPerson.ProfessionList.Count);
            Assert.True(specificPerson.ProfessionList.Contains("actress"));
            Assert.Equal(3, specificPerson.KnownForList.Count);
            Assert.Contains("Lost in Translation", specificPerson.KnownForList.First().Title);

        }


        [Fact]
        public void GetSpecificPersonReturnsCompleteRecordOnValidName()
        {
            var service = new DataserviceSpecificPerson();
            var specificPerson = service.GetSpecificPersonByName("Scarlett Johansson");
            Assert.Contains("nm0424060", specificPerson.NConst);
            Assert.Equal(3, specificPerson.ProfessionList.Count);
            Assert.True(specificPerson.ProfessionList.Contains("actress"));
            Assert.Equal(3, specificPerson.KnownForList.Count);
            Assert.Contains("Lost in Translation", specificPerson.KnownForList.First().Title);
        }

        [Fact]
        public void GetSearchActorsReturnsActorListAndAddsSearchToUserSearchTable()
        {
            var service = new DataserviceSearches();
            var actorSearch = service.GetSearchResultActors("Tester4000", "scarlett");
            Assert.Contains("Scarlett", actorSearch.First().PrimaryName);
            Assert.Equal(13, actorSearch.Count);

            using var db = new PortfolioDBContext();
            Assert.NotNull(db.UserSearches.Where(x => x.Content == "scarlett").Where(x => x.Username == "Tester4000").FirstOrDefault());

            service.ClearSearchHistory("Tester4000");
        }

        [Fact]
        public void GetSearchTitlesReturnsTitlesListAndAddsSearchToUserSearchTable()
        {
            var service = new DataserviceSearches();
            var search = "twilight apple";
            var titleSearch = service.GetSearchResultTitles("Tester4000", search);
            Assert.NotNull(titleSearch);
            Assert.Equal(124, titleSearch.Count);

            using var db = new PortfolioDBContext();
            Assert.NotNull(db.UserSearches.Where(x => x.Content == search).Where(x => x.Username == "Tester4000").FirstOrDefault());

            service.ClearSearchHistory("Tester4000");

        }


        //Testing CRUD operation methods 
        [Fact]
        public void CreateUser_ValidData_CreateUserAndReturnNewObject()
        {
            var service = new DataserviceUsers();
            var newUser = new
            {
                Username = "Tester5000", 
                Email = "siemje@ruc.dk",
                Birthyear = 1998, 
                Password = "test1234"
            };


            var createUser = service.CreateUser();
            var hashResult = _hashing.Hash(registerModel.Password);

            var deleteUser = service.DeleteUser("Tester4001");


            Assert.NotNull(deleteUser);

            var category = service.CreateCategory("Test", "DeleteCategory_ValidId_RemoveTheCategory");
            var result = service.DeleteCategory(category.Id);
            Assert.True(result);
            category = service.GetCategory(category.Id);
            Assert.Null(category);
        }

        [Fact]
        public void DeleteUser_ValidId_RemoveTheUserFromDatabase()
        {
            var service = new DataserviceUsers();
           
            var deleteUser = service.DeleteUser("Tester4000");


            Assert.NotNull(deleteUser);

            var category = service.CreateCategory("Test", "DeleteCategory_ValidId_RemoveTheCategory");
            var result = service.DeleteCategory(category.Id);
            Assert.True(result);
            category = service.GetCategory(category.Id);
            Assert.Null(category);
        }

    }
}