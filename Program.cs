using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace WebScrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = new Uri("https://nlp.hiringbull.com");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                //usually i make a standard request without authentication, eg: to the home page.
                //by doing this request you store some initial cookie values, that might be used in the subsequent login request and checked by the server
                var homePageResult = client.GetAsync("/App/Dashboard/Index");
                homePageResult.Result.EnsureSuccessStatusCode();

                var content = new FormUrlEncodedContent(new[]
                {
                    //the name of the form values must be the name of <input /> tags of the login form, in this case the tag is <input type="text" name="username">
                    new KeyValuePair<string, string>("Email", "xyz@gmail.com"),
                    new KeyValuePair<string, string>("Password", "XYZ"),
                });
                var loginResult = client.PostAsync("/App/Account/Login", content).Result;
                loginResult.EnsureSuccessStatusCode();

                //make the subsequent web requests using the same HttpClient object
               string Content = client.GetStringAsync("/App/Dashboard/Index").Result;
            }
        }
    }
}
