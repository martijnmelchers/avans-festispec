using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Festispec.Web.Models
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies["CurrentUserId"] == null && !httpContext.Request.Path.Equals("/"))
                httpContext.Response.Redirect("/");
            else
                await _next.Invoke(httpContext);
        }
    }
}