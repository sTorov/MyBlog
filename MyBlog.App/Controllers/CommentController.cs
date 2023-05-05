using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Comments.Response;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер комментариев
    /// </summary>
    [Authorize, CheckUserId]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> Create(CommentCreateViewModel model)
        {
            var result = await _commentService.CreateCommentAsync(model);
            if (!result)
                return BadRequest();
            
            return RedirectToAction("View", "Post", 
                new { Id = model.PostId, UserId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value });
        }

        /// <summary>
        /// Страница всех комментариев (получение комментариев для указанной статьи)
        /// </summary>
        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        [Route("GetComments/{postId?}")]
        public async Task<IActionResult> GetComments([FromRoute] int? postId, [FromQuery] int? userId)
        {
            var model = await _commentService.GetCommentsViewModelAsync(postId, userId);
            if(model == null)
                return BadRequest();

            return View(model);
        }

        /// <summary>
        /// Страница редактирования комментария
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int? userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var model = await _commentService.GetCommentEditViewModelAsync(id, userId, access);

            if (model == null)
                return BadRequest();

            return View(model);
        }

        /// <summary>
        /// Редактирование комментария
        /// </summary>
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

        /// <summary>
        /// Удаление комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromForm] int? userId, string? returnUrl)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var result = await _commentService.DeleteCommentAsync(id, userId, access);
            if(!result)
                return BadRequest();

            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl + $"?userId={userId}");
            return RedirectToAction("GetComments");
        }
    }
}
