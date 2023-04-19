using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Comments;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, IPostService postService)
        {
            _commentService = commentService;
            _postService = postService;

        }

        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> Create(CommentCreateViewModel model)
        {
            var result = await _commentService.CreateCommentAsync(model);
            if (!result)
                return BadRequest();

            return RedirectToAction("View", "Post", new { Id = model.PostId });
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        [Route("GetComments/{postId?}")]
        public async Task<IActionResult> GetComments([FromRoute] int? postId, [FromQuery] int? userId)
        {
            if(postId != null && await _postService.GetPostByIdAsync((int)postId) == null)
                return NotFound();

            var model = await _commentService.GetCommentsViewModelAsync(postId, userId);
                return View(model);
        }

        [CheckParameter(parameterName: "userId", path: "Comment/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int? userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var model = await _commentService.GetCommentEditViewModelAsync(id, userId, access);

            if (model == null)
                return BadRequest();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            var result = await _commentService.UpdateCommentAsync(model);
            if(!result)
                return BadRequest();

            if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);
            return RedirectToAction("GetComments");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromForm] int? userId, string? returnUrl)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var result = await _commentService.DeleteCommentAsync(id, userId, access);
            if(!result)
                return BadRequest();

            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("GetComments");
        }
    }
}
