using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [CheckUserId(parameterName: "userId", actionName: "CreatePost")]
        [HttpGet]
        [Route("CreatePost")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> PostCreate(PostCreateViewModel model)
        {
            var creater = await _postService.CheckDataAtCreated(this, model);

            if (ModelState.IsValid)
            {
                List<Tag> tags = null!;
                if (!string.IsNullOrEmpty(model.PostTags))
                    tags = await _postService.CreateTagAtPostAsync(model.PostTags);

                await _postService.CreatePost(creater!, model, tags);
                return RedirectToAction("GetPost");
            }
            else
                return View("Create", model);
        }

        [HttpGet]
        [Route("GetPost/{userId?}")]
        public async Task<IActionResult> GetPost([FromRoute]int? userId)
        {
            var model = await _postService.GetPostViewModel(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _postService.DeletePost(id);

            return RedirectToAction("GetPost");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _postService.GetPostEditViewModel(id);

            if (model == null)
                return RedirectToAction("GetPost");

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
