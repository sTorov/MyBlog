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

        [HttpGet]
        [Route("CreateComment")]
        public async Task<IActionResult> Create()
        {
            var result = await _commentService.CheckDataAtCreateComment(this);
            return result ?? View();
        }

        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> Create(CommentCreateViewModel model)
        {
            var result = await _commentService.CreateComment(model);

            if (!result)
                return BadRequest();

            return RedirectToAction("GetComment");
        }

        [CheckUserId(parameterName: "userId", actionName: "GetComment", fullAccess: "Admin, Moderator")]
        [HttpGet]
        [Route("GetComment/{postId?}")]
        public async Task<IActionResult> GetComment([FromRoute] int? postId)
        {
            if(postId != null && await _postService.GetPostByIdAsync((int)postId) == null)
                return NotFound();

            var model = await _commentService.GetCommentsViewModel(postId, Request.Query["userId"]);
                return View(model);
        }

        [CheckUserId(parameterName: "userId", actionName: "Comment/Edit", fullAccess: "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _commentService.GetCommentEditViewModel(id);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            _ = await _commentService.UpdateComment(model);

            return RedirectToAction("GetComment");
        }

        [CheckUserId(parameterName: "userId", fullAccess: "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _commentService.DeleteComment(id);

            return RedirectToAction("GetComment");
        }
    }
}
