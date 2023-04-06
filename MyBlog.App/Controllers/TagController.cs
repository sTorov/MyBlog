﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Tags;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Route("CreateTag")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("CreateTag")]
        public async Task<IActionResult> CreatePost(TagCreateViewModel model)
        {
            _ = await _tagService.CheckDataAtCreateTagAsync(this, model);

            if (ModelState.IsValid)
            {
                await _tagService.CreateTagAsync(model);
                return RedirectToAction("GetTag");
            }
            else
                return View("Create", model);
        }

        [HttpGet]
        [Route("GetTag/{id?}")]
        public async Task<IActionResult> GetTag([FromRoute] int? id)
        {
            var model = await _tagService.GetTagsViewModelAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _tagService.GetTagEditViewModelAsync(id);
            if (model == null)
                return RedirectToAction("GetTag");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            _ = await _tagService.CheckDataAtEditTagAsync(this, model);

            if (ModelState.IsValid)
            {
                await _tagService.UpdateTagAsync(model);
                return RedirectToAction("GetTag");
            }
            else
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _tagService.DeleteTagAsync(id);

            return RedirectToAction("GetTag");
        }
    }
}
