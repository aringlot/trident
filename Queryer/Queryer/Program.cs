using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using IdentityModel.Client;

namespace Queryer
{
    class Program
    {
        const string baseUrl = "http://localhost:60852";
        static TokenResponse GetUserToken(TokenClient client)
        {         
            return client.RequestResourceOwnerPasswordAsync("alice", "password", "api").Result;
        }

        static void CallApi(TokenResponse response)
        {
            var cookieContainer = new CookieContainer();
            using(var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                //client.SetBearerToken(response.AccessToken);
                var message = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/api/values");
                //message.Headers.Add("Cookie", $".MinsideApp={response.AccessToken};test={Guid.NewGuid()}");
                cookieContainer.Add(new Cookie(".MinsideApp", response.AccessToken) { HttpOnly = true, Domain="localhost" });
                cookieContainer.Add(new Cookie("test", Guid.NewGuid().ToString()) { HttpOnly = true, Domain = "localhost" });
                var apiResponse = client.SendAsync(message).Result;
                Console.WriteLine(apiResponse.Content.ReadAsStringAsync().Result);
                //Console.WriteLine(client.GetStringAsync(baseUrl + "/api/values").Result);
            }
        }

        static void Main(string[] args)
        {
            using (var client = new TokenClient(
                baseUrl + "/idsrv/token",
                "api",
                "secret"))
            {
                var tokenResponse = GetUserToken(client);
                CallApi(tokenResponse);
                
               Console.ReadLine();
            }
        }
    }
}
