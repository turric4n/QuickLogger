using System;
using Microsoft.AspNetCore.Builder;
using QuickLogger.AspNet.Extensions;

namespace Microsoft.Extensions
{
    public static class UseKestrelLogging
    {
        public static IApplicationBuilder UseKestrelLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<KestrelLoggingMiddleware>();
        }
    }
}
