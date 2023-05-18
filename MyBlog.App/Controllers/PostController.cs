using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Posts.Response;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер статей
    /// </summary>
    [Authorize, CheckUserId]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly ICommentService _commentService;
        private readonly ICheckDataService _checkDataService;

        public PostController(IPostService postService, ITagService tagService, ICommentService commentService, ICheckDataService checkDataService)
        {
            _postService = postService;
            _tagService = tagService;
            _commentService = commentService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Страница создания статьи
        /// </summary>
        [HttpGet]
        [Route("CreatePost")]
        public async Task<IActionResult> Create()
        {
            var model = new PostCreateViewModel { AllTags = await _tagService.GetAllTagsAsync() };
            return View(model);
        }

        /// <summary>
        /// Создание статьи
        /// </summary>
        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _postService.CreatePostAsync(model);

                if (result)
                    return RedirectToAction("View", new { Id = await _postService.GetLastCreatePostIdByUserId(model.UserId), model.UserId });
                else
                    ModelState.AddModelError(string.Empty, "Ошибка! Не удалось создать статью!");
            }

            model.AllTags ??= await _tagService.GetAllTagsAsync();
            return View(model);
        }

        /// <summary>
        /// Страница всех статей (получения статей, имеющих указанный тег)
        /// </summary>
        [HttpGet]
        [Route("GetPosts/{tagId?}")]
        public async Task<IActionResult> GetPosts([FromRoute]int? tagId, [FromQuery] int? userId)
        {
            var model = await _postService.GetPostsViewModelAsync(tagId, userId);
            return View(model);
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromForm] int userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var (result, isDeleted) = await _postService.DeletePostAsync(id, userId, access);

            if (isDeleted) 
                return RedirectToAction("GetPosts");
            else 
                return result!;
        }

        /// <summary>
        /// Страница редактирования статьи
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var fullAccess = User.IsInRole("Admin") || User.IsInRole("Moderator");

            var (model, result) = await _postService.GetPostEditViewModelAsync(id, userId, fullAccess);

            if (model == null) return result!;

            model.AllTags = await _tagService.GetAllTagsAsync();
            return View(model);
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            await _checkDataService.CheckDataForUpdatePostAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _postService.UpdatePostAsync(model);
                if (result)
                {
                    if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    return RedirectToAction("GetPosts");
                }
                else
                    ModelState.AddModelError(string.Empty, $"Ошибка! Не удалось обновить статью!");                
            }

            model.AllTags ??= await _tagService.GetAllTagsAsync();
            return View(model);
        }

        /// <summary>
        /// Страница отображения указанной статьи
        /// </summary>
        [HttpGet]
        [Route("ViewPost/{id}")]
        public async Task<IActionResult> View([FromRoute] int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var model = await _postService.GetPostViewModelAsync(id, userId ?? string.Empty);

            if(model == null) return NotFound();

            model.Comments = await _commentService.GetAllCommentsByPostIdAsync(id);
            return View(model);
        }
    }
}
