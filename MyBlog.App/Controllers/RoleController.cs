using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Roles;

namespace MyBlog.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("GetRoles/{id?}")]
        public async Task<IActionResult> GetRoles([FromRoute]int? id)
        {
            var model = await _roleService.GetRolesViewModelAsync(id);
            if(model == null) 
                return NotFound();

            return View(model);
        }

        [HttpGet]
        [Route("CreateRole")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> Create(RoleCreateViewModel model)
        {
            _ = await _roleService.CheckDataForCreateTagAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRoleAsync(model);
                if (!result)
                    return BadRequest();

                return RedirectToAction("GetRoles");
            }

            return View(model);
        }

        [HttpGet]
        [Route("EditRole")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _roleService.GetRoleEditViewModelAsync(id);
            if (model == null) 
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            var currentRole = await _roleService.CheckDataAtEditAsync(this, model);
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRoleAsync(currentRole!, model);
                if(!result) 
                    return BadRequest();

                return RedirectToAction("GetRoles");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if(!result)
                return BadRequest();

            return RedirectToAction("GetRoles");
        }
    }
}
