using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер ошибок
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Получение представления при статус коде 404
        /// </summary>
        [Route("NotFound")]
        public new IActionResult NotFound() => View();

        /// <summary>
        /// Получение представления при статус коде 400, 500
        /// </summary>
        [Route("BadRequest")]
        public new IActionResult BadRequest() => View();

        /// <summary>
        /// Получение представления при статус коде 403
        /// </summary>
        [Route("AccessDenied")]
        public IActionResult AccessDenied() => View();
    }
}
