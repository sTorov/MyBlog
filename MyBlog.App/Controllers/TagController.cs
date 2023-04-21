using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Tags.Response;

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
            _ = await _tagService.CheckTagNameAsync(this, model);
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
