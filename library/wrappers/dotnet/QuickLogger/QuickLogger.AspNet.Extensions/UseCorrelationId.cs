using System;
using Microsoft.AspNetCore.Builder;
using QuickLogger.AspNet.Extensions;

namespace Microsoft.Extensions
{
    public static class UseCorrelationIdExtensions
    {
        public static IApplicationBuilder UseLoggerCorrelationIdMiddleware(this IApplicationBuilder builder)
        {
            

            return builder.UseMiddleware<CorrelationMiddleware>();
        }
    }
}
