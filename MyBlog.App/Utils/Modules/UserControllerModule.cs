using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.App.Utils.Modules
{
    /// <summary>
    /// Модуль контроллера пользователей
    /// </summary>
    public class UserControllerModule : IUserControllerModule
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserControllerModule(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CheckDataAtCreationAsync(UserController controller, UserRegisterViewModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователь с никнеймом [{model.Login}] уже существует!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.EmailReg}] уже зарегистрирован!");
        }

        public async Task<User?> CheckDataAtEditionAsync(UserController controller, UserEditViewModel model)
        {
            var currentUser = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == model.Id);
            if (currentUser == null)
            {
                controller.ModelState.AddModelError(string.Empty, $"Произошла непредвиденная ошибка! Пользователь не найден!");
                return null;
            }

            var checkLogin = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkLogin != null && checkLogin != currentUser.UserName)
                controller.ModelState.AddModelError(string.Empty, $"Никнейм [{model.Login}] уже используется!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.Email))?.Email;
            if (checkEmail != null && checkEmail != currentUser.Email)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.Email}] уже зарегистрирован!");

            return currentUser;
        }

        public async Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
                controller.ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");

            return user;
        }

        public async Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller)
        {
            var dict = new Dictionary<string, bool>();
            var allRoles = await _roleManager.Roles.ToListAsync();

            foreach (var role in allRoles)
                dict.Add(role.Name!, controller.Request.Form[role.Name!] == "on");

            return dict;
        }
    }
}
