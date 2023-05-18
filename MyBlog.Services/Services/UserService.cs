using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Services.Extensions;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ViewModels.Users.Intefaces;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервисы сущности пользователя
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper, IRoleService roleService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<(bool, User)> CreateUserAsync(UserRegisterViewModel model, List<Role>? roles = null)
        {
            var user = _mapper.Map<User>(model);

            if (roles == null)
            {
                var defaultRole = await _roleService.GetRoleByNameAsync("User");
                if (defaultRole != null)
                    user.Roles = new List<Role> { defaultRole };
            }
            else
                user.Roles = roles;

            var result = await _userManager.CreateAsync(user, model.PasswordReg);

            return (result.Succeeded, user);
        }

        public async Task<bool> UpdateUserAsync(IUserUpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if(user == null) return false;

            user.Convert(model);
            user.Roles = await _roleService.ConvertRoleNamesInRoles(model.Roles);
            
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                var updateSecurityStampRes = await _userManager.UpdateSecurityStampAsync(user);
                return updateSecurityStampRes.Succeeded;
            }

            return false;
        }

        public async Task<List<User>> GetAllUsersAsync() => await _userManager.Users.Include(u => u.Roles).ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetUserByNameAsync(string name) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == name);

        public async Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if (check) return (await _userManager.DeleteAsync(user!)).Succeeded;
            return false;
        }

        public async Task<bool> DeleteByIdAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<(UserEditViewModel?, IActionResult?)> GetUserEditViewModelAsync(int id, string? userId, bool fullAccess)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return (null, new NotFoundResult());

            if (fullAccess || user.Id.ToString() == userId)
            {
                var model = _mapper.Map<UserEditViewModel>(user);
                model.AllRoles = (await _roleService.GetAllRolesAsync()).Select(r => r.Name!).ToList();
                return (model, null);
            }

            return (null, new ForbidResult());
        }

        public async Task<UsersViewModel?> GetUsersViewModelAsync(int? roleId)
        {
            var model = new UsersViewModel
            {
                Users = await _userManager.Users.Include(u => u.Roles).ToListAsync()
            };

            if (roleId != null)
                model.Users = model.Users
                    .SelectMany(u => u.Roles, (u, r) => new { User = u, RoleId = r.Id })
                    .Where(o => o.RoleId == roleId).Select(o => o.User).ToList();

            return model;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var claims = new List<Claim>();

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name!));
            claims.Add(new Claim("UserID", userId));

            return claims;
        }

        public async Task<UserViewModel?> GetUserViewModelAsync(int id)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;
            return _mapper.Map<UserViewModel>(user);
        }
    }
}
