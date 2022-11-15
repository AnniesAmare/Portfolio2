
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebServer.Model;
using WebServer.Controllers;
using System.Reflection.Metadata;

namespace Portfolio2.Tests
{
    
    public class WebServiceTests 
    {
        private const string registerUserAPI = "http://localhost:5001/api/user/register/";
        private const string loginUserAPI = "http://localhost:5001/api/user/login";

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

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }



    }
}
