using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
