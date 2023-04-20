using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IdentityResult> CreateUserAsync(UserCreateViewModel model)
        {
            var user = _mapper.Map<User>(model);
            user.Roles = await GetRoleListFromDictionary(model.AllRoles);

            var result = await _userManager.CreateAsync(user, model.PasswordReg);
            return result;
        }


        public async Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user)
        {
            user.Convert(model);
            user.Roles = await GetRoleListFromDictionary(model.AllRoles!);
            
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded) 
                return await _userManager.UpdateSecurityStampAsync(user);

            return result;
        }

        private async Task<List<Role>> GetRoleListFromDictionary(Dictionary<string, bool> dict)
        {
            var roles = new List<Role>();
            foreach (var pair in dict)
            {
                if (pair.Value)
                {
                    var role = await _roleManager.FindByNameAsync(pair.Key);
                    if (role != null)
                        roles.Add(role);
                }
            }
            return roles;
        }



        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userManager.UpdateAsync(user);

        public async Task<List<User>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) => 
            await Task.Run(() => _userManager.Users.Include(u => u.Roles).AsEnumerable().FirstOrDefault(u => u.Id == id));

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if (check) return (await _userManager.DeleteAsync(user!)).Succeeded;
            return false;
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id, int? userId, bool fullAccess)
        {
            var user = await GetUserByIdAsync(id);
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if(!check)
                return null;
            return _mapper.Map<UserEditViewModel>(user);
        }

        public async Task<UsersViewModel?> GetUsersViewModelAsync(int? id)
        {
            var model = new UsersViewModel();
            
            if (id == null)
                model.Users = await _userManager.Users.Include(u => u.Roles).ToListAsync();
            else
            {
                var user = await _userManager.FindByIdAsync(id?.ToString() ?? "");
                if (user != null) model.Users.Add(user);
            }

            return model;
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

        public async Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
                controller.ModelState.AddModelError(string.Empty, "Неверный email или(и) пароль!");

            return user;
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

        public async Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller)
        {
            var dict = new Dictionary<string, bool>();
            var allRoles = await _roleManager.Roles.ToListAsync();

            foreach (var role in allRoles)
                dict.Add(role.Name!, controller.Request.Form[role.Name!] == "on");

            return dict;
        }

        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var claims = new List<Claim>();

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name!));
            claims.Add(new Claim("UserID", userId));

            return claims;
        }

        public async Task<UserViewModel?> GetUserViewModelAsync(int id, int? userId, bool fullAccess)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if (!check) return null;
            return _mapper.Map<UserViewModel>(user);
        }
    }
}
