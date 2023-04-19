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
            var result = await _postService.DeletePost(id);
            if (!result)
                return BadRequest();

            return RedirectToAction("GetPost");
        }

        [CheckUserId(parameterName: "userId", actionName: "Post/Edit", fullAccess: "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _postService.GetPostEditViewModel(id, Request.Query["userId"]);
            if (model == null) 
                return NotFound();
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

                return RedirectToAction("GetPost");
            }

            model.AllTags = await _tagService.GetAllTagsAsync();
            return View(model);
        }
    }
}
