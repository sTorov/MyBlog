using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Attributes;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Roles.Response;

namespace MyBlog.App.Controllers
{
    /// <summary>
    /// Контроллер ролей
    /// </summary>
    [Authorize(Roles = "Admin"), CheckUserId]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ICheckDataService _checkDataService;

        public RoleController(IRoleService roleService, ICheckDataService checkDataService)
        {
            _roleService = roleService;
            _checkDataService = checkDataService;
        }

        /// <summary>
        /// Странмца отображения всех ролей (получение ролей указанного пользователя)
        /// </summary>
        [HttpGet]
        [Route("GetRoles/{userId?}")]
        public async Task<IActionResult> GetRoles([FromRoute]int? userId)
        {
            var model = await _roleService.GetRolesViewModelAsync(userId);
            if(model == null)
                return BadRequest();

            return View(model);
        }

        /// <summary>
        /// Страница создания роли
        /// </summary>
        [HttpGet]
        [Route("CreateRole")]
        public IActionResult Create() => View();

        /// <summary>
        /// Создание роли
        /// </summary>
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> Create(RoleCreateViewModel model)
        {
            _ = await _checkDataService.CheckDataForCreateRoleAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRoleAsync(model);
                if (!result)
                    return BadRequest();

                return RedirectToAction("GetRoles");
            }

            return View(model);
        }

        /// <summary>
        /// Страница редактирования роли
        /// </summary>
        [HttpGet]
        [Route("EditRole")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _roleService.GetRoleEditViewModelAsync(id);
            if (model == null)
                return BadRequest();

            return View(model);
        }

        /// <summary>
        /// Редактирование роли
        /// </summary>
        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            var currentRole = await _checkDataService.CheckDataForEditRoleAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRoleAsync(currentRole!, model);
                if(!result) 
                    return BadRequest();

                return RedirectToAction("GetRoles");
            }

            return View(model);
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if(!result)
                return BadRequest();

            return RedirectToAction("GetRoles");
        }

        /// <summary>
        /// Страница отображения указанной роли
        /// </summary>
        [HttpGet]
        [Route("/ViewRole/{id}")]
        public async Task<IActionResult> View([FromRoute] int id)
        {
            var model = await _roleService.GetRoleViewModel(id);
            if(model == null)
                return BadRequest();

            return View(model);
        }
    }
}
