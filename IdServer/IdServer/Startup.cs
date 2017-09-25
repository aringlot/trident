using System.Collections.Generic;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using IdServer.Config;
using Owin;

namespace IdServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                            .UseInMemoryUsers(Users.Get());

            var options = new IdentityServerOptions
            {
                Factory = factory,

                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}
