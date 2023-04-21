using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts.Response;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly ICommentService _commentService;

        public PostController(IPostService postService, ITagService tagService, ICommentService commentService)
        {
            _postService = postService;
            _tagService = tagService;
            _commentService = commentService;
        }

        [HttpGet]
        [Route("CreatePost")]
        public async Task<IActionResult> Create()
        {
            var model = new PostCreateViewModel { AllTags = await _tagService.GetAllTagsAsync() };
            return View(model);
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            var tags = await _tagService.CreateTagForPostAsync(model.PostTags);

            var result = await _postService.CreatePostAsync(model, tags);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Не удалось создать статью!");
                return View(model);
            }
            return RedirectToAction("GetPosts");
        }

        [HttpGet]
        [Route("GetPosts/{tagId?}")]
        public async Task<IActionResult> GetPosts([FromRoute]int? tagId, [FromQuery] int? userId)
        {
            var model = await _postService.GetPostsViewModelAsync(tagId, userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromForm] int userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var result = await _postService.DeletePostAsync(id, userId, access);

            if (!result) return BadRequest();

            return RedirectToAction("GetPosts");
        }

        [CheckParameter(parameterName: "userId", path: "Post/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromQuery] int? userId)
        {
            var access = User.IsInRole("Admin") || User.IsInRole("Moderator");
            var model = await _postService.GetPostEditViewModelAsync(id, userId, access);

            if (model == null) return BadRequest();

            model.AllTags = await _tagService.GetAllTagsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            var currentPost = await _postService.CheckDataAtUpdatePostAsync(this, model);
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
