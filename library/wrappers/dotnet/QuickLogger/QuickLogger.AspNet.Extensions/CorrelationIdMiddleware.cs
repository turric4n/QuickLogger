using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace QuickLogger.AspNet.Extensions
{
    internal class CorrelationMiddleware
    {
        internal const string CorrelationHeaderIdKey = "correlationId";

        internal const string CorrelationHeaderCallerNameKey = "correlationCallerName";

        internal const string CorrelationHeaderCallerMethodKey = "correlationCallerMethod";

        internal const string DefaultCallerName = "Undefined";

        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public CorrelationMiddleware(
            RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _loggerFactory = loggerFactory;
        }

        private void AddHeaderIfNotPresent(HttpContext context, string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerValue))
            {
                context.Request.Headers.Add(headerName, headerValue);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = _loggerFactory.CreateLogger<CorrelationMiddleware>();

            context.Request.Headers.TryGetValue(CorrelationHeaderIdKey, out var correlationId);

            context.Request.Headers.TryGetValue(CorrelationHeaderCallerNameKey, out var correlationCallerName);

            context.Request.Headers.TryGetValue(CorrelationHeaderCallerMethodKey, out var correlationCallerMethod);

            correlationId = string.IsNullOrEmpty(correlationId.ToString()) ? Guid.NewGuid().ToString() : correlationId;

            correlationCallerName = string.IsNullOrEmpty(correlationCallerName.ToString()) ? DefaultCallerName : correlationCallerName;

            correlationCallerMethod = string.IsNullOrEmpty(correlationCallerMethod.ToString()) ? DefaultCallerName : correlationCallerMethod;

            AddHeaderIfNotPresent(context, CorrelationHeaderIdKey, correlationId);

            AddHeaderIfNotPresent(context, CorrelationHeaderCallerNameKey, correlationCallerName);

            AddHeaderIfNotPresent(context, CorrelationHeaderCallerMethodKey, correlationCallerMethod);

            using var scope = (logger.BeginScope(
                new Dictionary<string, object>()
                {
                    {"correlationId", correlationId},
                    {"correlationCallerName", correlationCallerName},
                    {"correlationCallerMethod", correlationCallerMethod},
                }));

            await this._next.Invoke(context);
        }
    }
}
