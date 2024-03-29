﻿
using WebServer.Model;
using System.Net.Http.Headers;

namespace Portfolio2.Tests
{
    
    public class WebServiceTests 
    {
        //private const string registerUserAPI = "https://localhost:5001/api/user/register";
        private const string loginUserAPI = "https://localhost:5001/api/user/login";
        private const string CreateUnamedBookmarkAPI = "https://localhost:5001/api/user/bookmarks/create";
        private const string DeletBookmarAPI = "https://localhost:5001/api/user/bookmarks/delete";
        private const string GetMoviesAPI = "https://localhost:5001/api/titles/movies";
        private const string GetMoviesInvalidAPI = "https://localhost:5001/api/title/movies";
        private const string GetTitleByIdAPI = "https://localhost:5001/api/title/tt0052520";
        private const string GetTitleByIdInvalidAPI = "https://localhost:5001/api/title/XX0052520";
        private const string UpdateBookmarkAPI = "https://localhost:5001/api/user/bookmarks/rename";

        /* /api/...*/

        [Fact]
        public void ApiUserLogin_PostUser_LoggedIn()
        {      
            var userInfo = new UserLoginModel
            {
                Username = "Tester4000",
                Password = "tester1999",
            };

            var (User, statusCode) = PostData(loginUserAPI, userInfo);

            Assert.Equal("Tester4000", User["username"].ToString());
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Tester4000", User["username"].ToString());
            Assert.NotNull(User["token"].ToString());

        }


        //Testing WebAPI CRUD operations
        [Fact]
        public void ApiMovies_CompleteProduct()
        {
            var (movieList, statusCode) = GetObject(GetMoviesAPI);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("https://localhost:5001/api/titles/movies?page=0&pageSize=20", movieList["first"]);
            Assert.Equal("Dogville", movieList["items"].First()["name"]);
            Assert.Equal("The Stuff", movieList["items"].Last()["name"]);
        }

        [Fact]
        public void ApiMovies_CompleteProduct_InvalidResourcePath()
        {
            var (movieList, statusCode) = GetObject(GetMoviesInvalidAPI);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }      

        [Fact]
        public void ApiTitle_CompleteProduct()
        {
            var (movie, statusCode) = GetObject(GetTitleByIdAPI);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Drama", movie["genre"].First());
            Assert.Equal("Horror", movie["genre"].Last());

        }

        [Fact]
        public void ApiTitle_CompleteProduct_InvalidRequestTarget()
        {
            var (movie, statusCode) = GetObject(GetTitleByIdInvalidAPI);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }



        [Fact] 
        public void ApiCreateBookmark_ValidIdAndNameOK_AND_BookmarkDeletedOK()
        {
            var content = new 
            {
                id = "nm2552623",
                name = "Victy",
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
                id = "XXXX2623",
                name = "Victy",
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
        public void ApiBookmark_Update_Valid_Ok()
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

            //update bookmark
            var statusCode = PutDataWithAuthorization
                ($"{UpdateBookmarkAPI}/{update.id}/{update.name}", update.description);

            Assert.Equal(HttpStatusCode.OK, statusCode);


            //delete bookmark

            var statusCodeDelete = DeleteDataWithAuthorization
                ($"{DeletBookmarAPI}/{content.id}");

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);
        }

        [Fact]
        public void ApiBookmark_Update_Invalid()
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
                id = content.id + "Invalid NConst",
                name = content.name + "Updated",
                description = content.description + "Updated"
            };

            //update bookmark
            var statusCode = PutDataWithAuthorization
                ($"{UpdateBookmarkAPI}/{update.id}/{update.name}", update.description);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);


            //delete bookmark

            var statusCodeDelete = DeleteDataWithAuthorization
                ($"{DeletBookmarAPI}/{content.id}");

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);
        }



        // Helper functions
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
