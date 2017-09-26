using Microsoft.AspNetCore.Builder;
using Oceloter.Middleware;

namespace Oceloter.Extensions
{
    public static class ApplicatoinBuilderExtensions
    {
        public static IApplicationBuilder UseAuthorizationHeader(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationHeaderMiddleware>();
            return app;
        }

    }
}
