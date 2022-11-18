using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Portfolio2.Tests
{
    public class DataServiceTests
    {


        //Testing whether our repository methods return expected values

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
        public void GetSpecificTitleReturnsCompleteRecordOnInvalidTConst()
        {
            var service = new DataserviceSpecificTitle();
            var specificTitle = service.GetSpecificTitle("xx0052520");
            Assert.Null(specificTitle);
           
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
        public void GetSpecificTitleReturnsCompleteRecordOnInvalidName()
        {
            var service = new DataserviceSpecificTitle();
            var specificTitle = service.GetSpecificTitleByName("Lalalalalaa");
            Assert.Null(specificTitle);
           
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
        public void GetSpecificPersonReturnsCompleteRecordOnInvalidNConst()
        {
            var service = new DataserviceSpecificPerson();
            var specificPerson = service.GetSpecificPerson("mmmmm0424060");
            Assert.Null(specificPerson);
       
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
        public void GetSpecificPersonReturnsCompleteRecordOnInValidName()
        {
            var service = new DataserviceSpecificPerson();
            var specificPerson = service.GetSpecificPersonByName("Henny");
            Assert.Null(specificPerson);
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
        public void GetSearchActorsReturnsActorListAndAddsSearchToUserSearchTable_InvalidUser()
        {
            var service = new DataserviceSearches();
            var actorSearch = service.GetSearchResultActors("Tester8000", "scarlett");
            Assert.Null(actorSearch);
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

        [Fact]
        public void GetSearchTitlesReturnsTitlesListAndAddsSearchToUserSearchTable_WithInvalidUser()
        {
            var service = new DataserviceSearches();
            var search = "twilight apple";
            var titleSearch = service.GetSearchResultTitles("Tester8000", search);
            Assert.Null(titleSearch);

        }

        [Fact]
        public void InsertUserRating_CreateOrUpdate_WithValidUser()
        {
            var service = new DataserviceUserRatings();
            var NewRating = service.InsertUserRating("Tester4000", "tt0052520", 3);

            Assert.True(NewRating);
        }

        [Fact]
        public void InsertUserRating_CreateOrUpdate_WithInvalidUser()
        {
            var service = new DataserviceUserRatings();
            var NewRating = service.InsertUserRating("Tester8000", "tt0052520", 3);

            Assert.False(NewRating);
        }

        [Fact]
        public void GetUserRatingReturnsUserRatingElementList()
        {
            var service = new DataserviceUserRatings();
            service.InsertUserRating("Tester4000", "tt0052520", 7);
            var UserRatings = service.GetUserRatings("Tester4000", 0, 20);

            //Both asserts look for expected return types (UI)
            Assert.IsType<List<UserRatingElement>>(UserRatings);
            Assert.NotNull(UserRatings);

            Assert.Equal("The Twilight Zone", UserRatings.First().Title);
            Assert.Equal(1, UserRatings.Count);
        }

        [Fact]
        public void GetUserRatingReturnsUserRatingElementList_WithInValidUser()
        {
            var service = new DataserviceUserRatings();
            var UserRatings = service.GetUserRatings("Tester8000", 0, 20);

            Assert.Empty(UserRatings);
            
        }


        [Fact]
        public void DeleteUserRatings_WithValidUser()
        {
            var service = new DataserviceUserRatings();
            var DeleteRating = service.DeleteUserRatings("Tester4000", "tt0052520");

            Assert.True(DeleteRating);
        }

        [Fact]
        public void DeleteUserRatings_WithInvalidUser()
        {
            var service = new DataserviceUserRatings();
            var DeleteRating = service.DeleteUserRatings("Tester8000", "tt0052520");

            Assert.False(DeleteRating);
        }



    }
}