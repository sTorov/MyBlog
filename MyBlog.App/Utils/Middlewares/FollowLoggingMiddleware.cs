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
            var userName = context.Session.GetString("username") ?? "_";

            _logger.Info($"{userName} => {path}");

            await _next.Invoke(context);
        }
    }
}
