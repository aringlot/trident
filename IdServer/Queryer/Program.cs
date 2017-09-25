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
        static TokenResponse GetClientToken()
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "silicon",
                "secret");

            return client.RequestClientCredentialsAsync("api").Result;
        }

        static TokenResponse GetUserToken()
        {
            var client = new TokenClient(
                "http://localhost:60852/idsrv/token",
                "carbon",
                "secret");
            
            return client.RequestResourceOwnerPasswordAsync("bob", "secret", "api offline_access").Result;
        }

        static void CallApi(TokenResponse response)
        {
            using (var client = new HttpClient())
            {
                client.SetBearerToken(response.AccessToken);

                Console.WriteLine(client.GetStringAsync("http://localhost:60219/test").Result);
            }
        }

        static TokenResponse RefreshToken(string token)
        {
            var client = new TokenClient(
                "http://localhost:5000/connect/token",
                "carbon",
                "secret");
            
            return client.RequestRefreshTokenAsync(token).Result;
        }

        static void Main(string[] args)
        {
            var tokenResponse = GetUserToken();
            Console.WriteLine(tokenResponse.AccessToken);
            tokenResponse = RefreshToken(tokenResponse.RefreshToken);
            Console.ReadLine();
        }
    }
}
