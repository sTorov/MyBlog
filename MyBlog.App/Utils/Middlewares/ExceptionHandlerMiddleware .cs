using NLog;

namespace MyBlog.App.Utils.Middlewares
{
    /// <summary>
    /// Промежуточное ПО для обработки и логирования исключений
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NLog.ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " >>>>>> " + e.TargetSite?.DeclaringType?.FullName + "." + e.TargetSite?.Name);
                context.Response.Redirect("/BadRequest");
            }
        }
    }
}
