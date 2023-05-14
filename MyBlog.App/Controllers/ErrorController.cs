using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер ошибок
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Получение представления при статус коде 404
        /// </summary>
        [HttpGet]
        [Route("NotFound")]
        public new IActionResult NotFound() => View();

        /// <summary>
        /// Получение представления при статус коде 400, 500
        /// </summary>
        [HttpGet]
        [Route("BadRequest")]
        public new IActionResult BadRequest() => View();

        /// <summary>
        /// Получение представления при статус коде 403
        /// </summary>
        [HttpGet]
        [Route("AccessDenied")]
        public IActionResult AccessDenied() => View();
    }
}
