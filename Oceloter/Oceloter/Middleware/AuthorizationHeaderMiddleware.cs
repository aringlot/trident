using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Ocelot.Logging;
using Oceloter.Models;

namespace Oceloter.Middleware
{
    public class AuthorizationHeaderMiddleware
    {
        private const string AuthorizationHeader = "Authorization";
        private const string AuthorizationCookie = ".MinsideApp";
        private readonly Regex _identityServerPath;
        private const int HttpSuccess = 200;

        private readonly RequestDelegate next;

        private readonly IOcelotLogger logger;

        public AuthorizationHeaderMiddleware(
            RequestDelegate next,
            IOcelotLoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger <AuthorizationHeaderMiddleware>();
            _identityServerPath = new Regex("idsrv/token");
        }

        public async Task Invoke(HttpContext context)
        {
            var cookie = context.Request.Cookies[AuthorizationCookie];

            if (cookie != null)
            {
                context.Request.Headers.Remove(AuthorizationHeader);
                context.Request.Headers.Add(AuthorizationHeader, new Microsoft.Extensions.Primitives.StringValues($"Bearer {cookie}"));
            }

            var bodyOriginalStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                if (_identityServerPath.IsMatch(context.Request.Path) && context.Response.StatusCode == HttpSuccess)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var result = Encoding.UTF8.GetString(responseBody.ToArray());

                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

                    if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        context.Response.Cookies.Append(AuthorizationCookie,
                            tokenResponse.AccessToken,
                            new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
                            });
                    }

                }

                responseBody.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(bodyOriginalStream);
            }

        }
    }
}
