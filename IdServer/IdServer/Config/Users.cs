using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core.Services.InMemory;

namespace IdServer.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
        {
            new InMemoryUser
            {
                Username = "bob",
                Password = "secret",
                Subject = "1",
                Claims = new List<Claim>
                {
                    new Claim("UserType", "registered")
                }
            },
            new InMemoryUser
            {
                Username = "alice",
                Password = "secret",
                Subject = "2",
                Claims = new List<Claim>
                {
                    new Claim("UserType", "registered")
                }
            }
        };
        }
    }
}
