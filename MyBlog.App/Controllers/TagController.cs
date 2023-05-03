using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Tags.Response;

namespace MyBlog.App.Controllers
{
    [CheckUserId]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ITagControllerModule _module;

        public TagController(ITagService tagService, ITagControllerModule module)
        {
            _tagService = tagService;
            _module = module;
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
            _ = await _module.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _tagService.CreateTagAsync(model);
                if (!result)
                    return BadRequest();

                return RedirectToAction("GetTags");
            }
            else
                return View(model);
        }

        [Authorize]
        [HttpGet]
        [Route("GetTags/{id?}")]
        public async Task<IActionResult> GetTags([FromRoute] int? id, [FromQuery] int? postId)
        {
            var model = await _tagService.GetTagsViewModelAsync(id, postId);
            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var model = await _tagService.GetTagEditViewModelAsync(id);
            if (model == null)
                return BadRequest();

            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            _ = await _module.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _tagService.UpdateTagAsync(model);
                if (!result) 
                    return BadRequest();

                return RedirectToAction("GetTags");
            }
            else
                return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var reselt = await _tagService.DeleteTagAsync(id);
            if(!reselt)
                return BadRequest();

            return RedirectToAction("GetTags");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> View([FromRoute]int id)
        {
            var model = await _tagService.GetTagViewModelAsync(id);
            if(model == null)
                return BadRequest();

            return View(model);
        }
    }
}
