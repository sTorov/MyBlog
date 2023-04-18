using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Roles;

namespace MyBlog.App.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("/GetRole/{id?}")]
        public async Task<IActionResult> GetRole(int? id)
        {
            var model = await _roleService.GetRolesViewModelAsync(id);

            if(model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        [Route("/CreateRole")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("/CreateRole")]
        public async Task<IActionResult> Create(RoleCreateViewModel model)
        {
            _ = await _roleService.CheckDataForCreateTag(this, model);

            if (ModelState.IsValid)
            {
                await _roleService.CreateRoleAsync(model);
                return RedirectToAction("GetRole");
            }

            return View(model);
        }
    }
}
