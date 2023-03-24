using Microsoft.AspNetCore.Mvc;

namespace MyBlog.App.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
