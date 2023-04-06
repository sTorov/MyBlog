﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Comments;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("CreateComment")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> PostCreate(CommentCreateViewModel model)
        {
            var (user, post) = await _commentService.CheckDataAtCreateComment(this, model);

            if (ModelState.IsValid)
            {
                await _commentService.CreateComment(user!, post!, model);
                return RedirectToAction("GetComment");
            }
            else
                return View("Create", model);
        }

        [HttpGet]
        [Route("GetComment/{postId?}")]
        public async Task<IActionResult> GetComment([FromRoute] int? postId)
        {
            var model = await _commentService.GetCommentsViewModel(postId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _commentService.GetCommentEditViewModel(id);

            if (model == null)
                return RedirectToAction("GetComment");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            _ = await _commentService.UpdateComment(model);

            return RedirectToAction("GetComment");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            _ = await _commentService.DeleteComment(id);

            return RedirectToAction("GetComment");
        }
    }
}
