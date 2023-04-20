using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    public class ErrorController : Controller
    {
        [Route("NotFound")]
        public new IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        [Route("BadRequest")]
        public new IActionResult BadRequest() 
        {
            Response.StatusCode = 400;
            return View();
        }

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}
