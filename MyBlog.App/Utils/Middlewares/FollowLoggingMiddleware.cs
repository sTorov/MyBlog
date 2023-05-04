using Microsoft.AspNetCore.Http.Extensions;
using NLog;

namespace MyBlog.App.Utils.Middlewares
{
    /// <summary>
    /// Промежуточное ПО для логирования действий пользователя
    /// </summary>
    public class FollowLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NLog.ILogger _logger;

        public FollowLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.GetEncodedUrl();
            _logger.Info(path);

            await _next.Invoke(context);
        }
    }
}
