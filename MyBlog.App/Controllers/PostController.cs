using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
