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
            using (var client = new HttpClient())
            {
                client.SetBearerToken(response.AccessToken);

                Console.WriteLine(client.GetStringAsync(baseUrl + "/api/values").Result);
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
