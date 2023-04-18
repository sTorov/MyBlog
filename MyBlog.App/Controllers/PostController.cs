using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;

        public PostController(IPostService postService, ITagService tagService)
        {
            _postService = postService;
            _tagService = tagService;
        }

        [CheckUserId(parameterName: "userId", actionName: "CreatePost")]
        [HttpGet]
        [Route("CreatePost")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            var tags = await _tagService.CreateTagForPostAsync(model.PostTags);

            var result = await _postService.CreatePost(model, tags);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Не удалось создать статью!");
                return View(model);
            }
            return RedirectToAction("GetPost");
        }

        [HttpGet]
        [Route("GetPost/{id?}")]
        public async Task<IActionResult> GetPost([FromRoute] int? id)
        {
            var model = await _postService.GetPostViewModel(id, Request.Query["userId"]);

            return View(model);
        }

        [CheckUserId(parameterName: "userId", fullAccess: "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _postService.DeletePost(id);

            return RedirectToAction("GetPost");
        }

        [CheckUserId(parameterName: "userId", actionName: "Post/Edit", fullAccess: "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var r = Request.RouteValues;

            var model = await _postService.GetPostEditViewModel(id, Request.Query["userId"]);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            _ = await _postService.UpdatePostAsync(model);

            return RedirectToAction("GetPost");
        }
    }
}
