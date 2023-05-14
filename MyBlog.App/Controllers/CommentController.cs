using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Comments.Response;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер комментариев
    /// </summary>
    [Authorize, CheckUserId]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, IPostService postService)
        {
            _commentService = commentService;
            _postService = postService;
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> Create(CommentCreateViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;

            var result = await _commentService.CreateCommentAsync(model);
            if (result)
                return RedirectToAction("View", "Post", new { Id = model.PostId, UserId = userId });
            else
            {
                ModelState.AddModelError(string.Empty, $"Ошибка! Не удалось создать комментарий!");

                var postViewModel = await _postService.GetPostViewModelAsync(model.PostId, userId ?? string.Empty);
                if (postViewModel == null) return NotFound();

                postViewModel.CommentCreateViewModel = model;
                return View("/Views/Post/View.cshtml", postViewModel);
            }            
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
            if(model == null) return NotFound();

            return View(model);
        }

        /// <summary>
        /// Страница редактирования комментария
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var fullAccess = User.IsInRole("Admin") || User.IsInRole("Moderator");

            var (model, result) = await _commentService.GetCommentEditViewModelAsync(id, userId, fullAccess);

            if (model == null) return result!;

            return View(model);
        }

        /// <summary>
        /// Редактирование комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            var result = await _commentService.UpdateCommentAsync(model);
            if (result)
            {
                if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("GetComments");
            }
            else
                ModelState.AddModelError(string.Empty, $"Ошибка! Не удалось обновить комментарий!");
            
            return View(model);
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromForm] int? userId, string? returnUrl)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var (result, isDeleted) = await _commentService.DeleteCommentAsync(id, userId, access);

            if(!isDeleted) return result!;

            if (returnUrl != null && Url.IsLocalUrl(returnUrl)) 
                return Redirect(returnUrl + $"?userId={userId}");
            return RedirectToAction("GetComments");
        }
    }
}
