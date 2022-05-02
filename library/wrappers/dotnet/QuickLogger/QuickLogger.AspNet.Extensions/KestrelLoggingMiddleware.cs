using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace QuickLogger.AspNet.Extensions
{
    internal class KestrelLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public KestrelLoggingMiddleware(
            RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _loggerFactory = loggerFactory;
        }

        private string GetIpValue(HttpContext context)
        {
            var ipAdd = context.Request.Headers["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = context.Request.Headers["REMOTE_ADDR"];
            }

            return ipAdd;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = _loggerFactory.CreateLogger<KestrelLoggingMiddleware>();

            await this._next.Invoke(context);

            var requestTrace = $"{GetIpValue(context)} - {context.Request.Host} - {context.Request.Path} - {context.Request.QueryString}" +
                               $" - {context.Request.Method} - {context.Request.ContentType}" +
                               $" - {context.Request.ContentLength}" +
                               $" - {context.Response.ContentLength} - {context.Response.ContentType}";

            logger.LogInformation(requestTrace);
        }
    }
}
