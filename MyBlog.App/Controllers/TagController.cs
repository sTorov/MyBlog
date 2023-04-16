using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Tags;

namespace MyBlog.App.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [Authorize]
        [HttpGet]
        [Route("CreateTag")]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        [Route("CreateTag")]
        public async Task<IActionResult> Create(TagCreateViewModel model)
        {
            _ = await _tagService.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                await _tagService.CreateTagAsync(model);
                return RedirectToAction("GetTag");
            }
            else
                return View(model);
        }

        [Authorize]
        [HttpGet]
        [Route("GetTag/{id?}")]
        public async Task<IActionResult> GetTag([FromRoute] int? id)
        {
            var model = await _tagService.GetTagsViewModelAsync(id, Request.Query["postId"]);

            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _tagService.GetTagEditViewModelAsync(id);

            if (model == null) return NotFound();

            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            _ = await _tagService.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                await _tagService.UpdateTagAsync(model);
                return RedirectToAction("GetTag");
            }
            else
                return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _tagService.DeleteTagAsync(id);

            return RedirectToAction("GetTag");
        }
    }
}
