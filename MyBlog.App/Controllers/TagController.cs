using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Tags.Request;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер тегов
    /// </summary>
    [CheckUserId]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ICheckDataService _checkDataService;

        public TagController(ITagService tagService, ICheckDataService checkDataService)
        {
            _tagService = tagService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Страница создания тега
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CreateTag")]
        public IActionResult Create() => View();

        /// <summary>
        /// Создание тега
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("CreateTag")]
        public async Task<IActionResult> Create(TagCreateViewModel model)
        {
            await _checkDataService.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _tagService.CreateTagAsync(model);
                if (result)
                    return RedirectToAction("GetTags");
                else
                    ModelState.AddModelError(string.Empty, $"Ошибка! Не удалось создать тег!");
            }
            
            return View(model);
        }

        /// <summary>
        /// Страница всех тегов (получение тегов для указанной статьи, получение указанного тега)
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("GetTags/{id?}")]
        public async Task<IActionResult> GetTags([FromRoute] int? id, [FromQuery] int? postId)
        {
            var model = await _tagService.GetTagsViewModelAsync(id, postId);
            return View(model);
        }

        /// <summary>
        /// Страница редактирования тега
        /// </summary>
        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var model = await _tagService.GetTagEditViewModelAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        /// <summary>
        /// Редактирование тега
        /// </summary>
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            await _checkDataService.CheckTagNameAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _tagService.UpdateTagAsync(model);
                if (result)
                    return RedirectToAction("GetTags");
                else
                    ModelState.AddModelError(string.Empty, $"Ошибка! Не удалось обновить тег!");
            }
            
            return View(model);
        }

        /// <summary>
        /// Удаление тега
        /// </summary>
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var reselt = await _tagService.DeleteTagAsync(id);
            if(!reselt) return BadRequest();

            return RedirectToAction("GetTags");
        }

        /// <summary>
        /// Страница отображения указанного тега
        /// </summary>
        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> View([FromRoute]int id)
        {
            var model = await _tagService.GetTagViewModelAsync(id);
            if(model == null) return NotFound();

            return View(model);
        }
    }
}
