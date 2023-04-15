using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<(IdentityResult, User)> CreateUserAsync(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            
            var defaultRole = await _roleManager.FindByNameAsync("User");
            if (defaultRole != null) 
                user.Roles = new List<Role> { defaultRole };

            var result = await _userManager.CreateAsync(user, model.PasswordReg);

            return (result, user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user)
        {
            user.Convert(model);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userManager.UpdateAsync(user);

        public async Task<List<User>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) => 
            await Task.Run(() => _userManager.Users.Include(u => u.Roles).AsEnumerable().FirstOrDefault(u => u.Id == id));

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetUserByNameAsync(string name) => await _userManager.FindByNameAsync(name);

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null) 
                return (await _userManager.DeleteAsync(user)).Succeeded;
            return false;
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if(user != null)
                return _mapper.Map<UserEditViewModel>(user);
            return null;
        }

        public async Task<UsersViewModel?> GetUsersViewModelAsync(int? id)
        {
            var model = new UsersViewModel();
            
            if (id == null)
                model.Users = await _userManager.Users.ToListAsync();
            else
            {
                var user = await _userManager.FindByIdAsync(id?.ToString() ?? "");
                if (user != null) model.Users.Add(user);
            }

            return model;
        }

        public async Task CheckDataAtRegistration(UserController controller, UserRegisterViewModel model)
        {
            var checkName = (await _userManager.FindByNameAsync(model.Login))?.UserName;
            if (checkName != null)
                controller.ModelState.AddModelError(string.Empty, $"Пользователь с никнеймом [{model.Login}] уже существует!");

            var checkEmail = (await _userManager.FindByEmailAsync(model.EmailReg))?.Email;
            if (checkEmail != null)
                controller.ModelState.AddModelError(string.Empty, $"Адрес [{model.EmailReg}] уже зарегистрирован!");
        }

        public async Task<User?> CheckDataAtEdition(UserController controller, UserEditViewModel model)
        {
            var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());
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
    }
}
