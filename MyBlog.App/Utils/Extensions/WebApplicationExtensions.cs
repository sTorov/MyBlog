using MyBlog.App.Utils.Middlewares;

namespace MyBlog.App.Utils.Extensions
{
    /// <summary>
    /// Расширения для веб-приложения
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Добавление промежуточного ПО обработки и логирования исключений
        /// </summary>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlerMiddleware>();
            return builder;
        }

        /// <summary>
        /// Добавление промежуточного ПО логирования действий пользователя
        /// </summary>
        public static IApplicationBuilder UseFollowLogging(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<FollowLoggingMiddleware>();
            return builder;
        }
    }
}
