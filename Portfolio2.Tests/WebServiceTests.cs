
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebServer.Model;
using WebServer.Controllers;
using System.Reflection.Metadata;
using System.Net.Http.Headers;

namespace Portfolio2.Tests
{
    
    public class WebServiceTests 
    {
        private const string registerUserAPI = "http://localhost:5001/api/user/register";
        private const string loginUserAPI = "http://localhost:5001/api/user/login";
        private const string GetUserAPI = "http://localhost:5001/api/user";
        private const string CreateUnamedBookmarkAPI = "http://localhost:5001/api/user/bookmarks/create";
        private const string DeletBookmarAPI = "http://localhost:5001/api/user/bookmarks/delete";
        private const string GetMoviesAPI = "http://localhost:5001/api/titles/movies";
        private const string UpdateBookmarkAPI = "http://localhost:5001/api/user/bookmarks/rename";

        /* /api/...*/

        [Fact]
        public void ApiUserLogin_PostUser_LoggedIn()
        {      
            var userInfo = new UserLoginModel
            {
                Username = "Tester4000",
                Password = "tester",
            };

            var (User, statusCode) = PostData(loginUserAPI, userInfo);

            Assert.Equal("Tester4000", User["username"].ToString());
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Tester4000", User["username"].ToString());
            Assert.NotNull(User["token"].ToString());

        }
#if COMMENT
        //testing Get Methods
        [Fact]
        public void ApiMovies_CompleteProduct()
        {
            var (movieList, statusCode) = GetObject(GetMoviesAPI);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("http://localhost:5001/api/titles/movies?page=0&pageSize=20", movieList["first"]);
            Assert.Equal("Dogville", movieList["items"].First()["name"]);
            Assert.Equal("The Stuff", movieList["items"].Last()["name"]);
        }


        //Testing WebAPI CRUD operations

        [Fact] //note that this test will fail if you already have created this test bookmark
        public void ApiCreateBookmark_ValidIdAndNameOK_AND_BookmarkDeletedOK()
        {
            var content = new 
            {
                id = "nm0424060",
                name = "ScarJo",
                description = "Bookmark Created!"
            };

            var (response, statusCodeCreated) = 
                PostDataWithAuthorization
                ($"{CreateUnamedBookmarkAPI}/{content.id}/{content.name}", content.description);

            Assert.Equal(HttpStatusCode.OK, statusCodeCreated);

            var statusCodeDelete = DeleteDataWithAuthorization
                ($"{DeletBookmarAPI}/{content.id}");

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);
        }

        [Fact] 
        public void ApiCreateBookmark_InvalidIdAndName_BadRequest()
        {
            var content = new
            {
                id = "nm042406123",
                name = "ScarJo",
                description = "Bookmark Created"
            };

            var (response, statusCode) =
                PostDataWithAuthorization(
                    $"{CreateUnamedBookmarkAPI}/{content.id}/{content.name}", content);

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void ApiBookmark_DeleteWithInvalidId_NotFound()
        {
            var statusCodeDelete = DeleteDataWithAuthorization
                ($"{DeletBookmarAPI}/NotValid12434");

            Assert.Equal(HttpStatusCode.NotFound, statusCodeDelete);
        }

        [Fact]
        public void ApiBookmark_UpdateWithValid_Ok()
        {

            var content = new
            {
                id = "nm0189075",
                name = "Alan Crossland",
                description = "Bookmark Created"
            };


            //create bookmark 
            var (bookmark, statusCodeCreated) =
               PostDataWithAuthorization(
                   $"{CreateUnamedBookmarkAPI}/{content.id}/{content.name}", 
                   content.description);

            Assert.Equal(HttpStatusCode.OK, statusCodeCreated);

            var update = new
            {
                id = content.id,
                name = content.name + "Updated",
                description = content.description + "Updated"
            };

            var statusCode = PutDataWithAuthorization
                ($"{UpdateBookmarkAPI}/{update.id}/{update.name}", update.description);

            Assert.Equal(HttpStatusCode.OK, statusCode);


            //delete bookmark

            var statusCodeDelete = DeleteDataWithAuthorization
                ($"{DeletBookmarAPI}/{content.id}");

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);
        }

    



        //user objects
        //var newUser = new UserRegisterModel
        //{
        //    Username = "Tester5000",
        //    Email = "siemje@ruc.dk",
        //    Birthyear = "1998",
        //    Password = "test1234"
        //};

        //var newUser = new UserRegisterModel
        //{
        //    Username = "Tester4000",
        //    Password = "tester1999",
        //    Email = "atru@ruc.dk",
        //    Birthyear = "1998"
        //};

#endif
        // Helpers
        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostDataWithAuthorization(string url, object content)
        {
            var userInfo = new UserLoginModel
            {
                Username = "Tester4000",
                Password = "tester",
            };

            var (User, statusCode1) = PostData(loginUserAPI, userInfo);
            var token = User["token"].ToString();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");

            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode PutDataWithAuthorization(string url, object content)
        {
            var userInfo = new UserLoginModel
            {
                Username = "Tester4000",
                Password = "tester",
            };

            var (User, statusCode1) = PostData(loginUserAPI, userInfo);
            var token = User["token"].ToString();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteDataWithAuthorization(string url)
        {
            var userInfo = new UserLoginModel
            {
                Username = "Tester4000",
                Password = "tester",
            };

            var (User, statusCodeGetToken) = PostData(loginUserAPI, userInfo);
            var token = User["token"].ToString();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }



    }
}
