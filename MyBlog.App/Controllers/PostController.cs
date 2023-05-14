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
            var tags = await _tagService.SetTagsForPostAsync(model.PostTags);

            var result = await _postService.CreatePostAsync(model, tags);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Не удалось создать статью!");
                return View(model);
            }
            return RedirectToAction("View", new { Id = await _postService.GetLastCreatePostIdByUserId(model.UserId), model.UserId });
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
            var result = await _postService.DeletePostAsync(id, userId, access);

            if (!result) return BadRequest();

            return RedirectToAction("GetPosts");
        }

        /// <summary>
        /// Страница редактирования статьи
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int? userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var model = await _postService.GetPostEditViewModelAsync(id, userId, access);

            if (model == null) return BadRequest();

            model.AllTags = await _tagService.GetAllTagsAsync();

            return View(model);
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            var currentPost = await _checkDataService.CheckDataForUpdatePostAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _postService.UpdatePostAsync(model, currentPost!);
                if (!result)
                    return BadRequest();

                if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("GetPosts");
            }

            model.AllTags = await _tagService.GetAllTagsAsync();
            return View(model);
        }

        /// <summary>
        /// Страница отображения указанной статьи
        /// </summary>
        [HttpGet]
        [Route("ViewPost/{id}")]
        public async Task<IActionResult> View([FromRoute] int id, [FromQuery] string userId)
        {
            var model = await _postService.GetPostViewModelAsync(id, userId);
            if(model == null)
                return BadRequest();

            model.Comments = await _commentService.GetAllCommentsByPostIdAsync(id);
            return View(model);
        }
    }
}
