using Microsoft.AspNetCore.Identity;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Roles.Response;

namespace MyBlog.App.Utils.Modules
{
    public class RoleControllerModule : IRoleControllerModule
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleControllerModule(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Role?> CheckDataAtEditAsync(RoleController controller, RoleEditViewModel model)
        {
            var checkRole = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (checkRole == null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с Id [{model.Id}] не найдена!");

            return checkRole;
        }

        public async Task<Role?> CheckDataForCreateTagAsync(RoleController controller, RoleCreateViewModel model)
        {
            var checkRole = await _roleManager.FindByNameAsync(model.Name);
            if (checkRole != null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с именем [{model.Name}] уже существует!");

            return checkRole;
        }
    }
}
