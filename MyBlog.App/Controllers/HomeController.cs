using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.ViewModels;
using System.Diagnostics;

namespace MyBlog.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string? returnUrl)
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("GetPosts", "Post");
            }
            return View();
        }

        public IActionResult Privacy() => View();

        [Authorize]
        [Route("AccessDenied")]
        public IActionResult AccessDenied() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}