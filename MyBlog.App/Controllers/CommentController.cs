using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Comments;

namespace MyBlog.App.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("CreateComment")]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> PostCreate(CommentCreateViewModel model)
        {
            var (user, post) = await _commentService.CheckDataAtCreateComment(this, model);

            if (ModelState.IsValid)
            {
                await _commentService.CreateComment(user!, post!, model);
                return RedirectToAction("GetComment");
            }
            else
                return View("Create", model);
        }

        [HttpGet]
        [Route("GetComment/{postId?}")]
        public async Task<IActionResult> GetComment([FromRoute] int? postId)
        {
            var model = await _commentService.GetCommentsViewModel(postId);

            return View(model);
        }
    }
}
