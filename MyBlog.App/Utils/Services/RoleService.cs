using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<Role>> GetRolesByUserAsync(int userId) =>
            await Task.Run(() => 
                _roleManager.Roles.Include(r => r.Users).AsEnumerable()
                .SelectMany(r => r.Users, (r, u) => new { Role = r, UserId = u.Id })
                .Where(o => o.UserId == userId).Select(o => o.Role).ToList()
            );

        public async Task<Role?> GetRoleByNameAsync(string roleName) => await _roleManager.FindByNameAsync(roleName);

        public async Task<UserRolesViewModel?> GetUserRolesViewModelAsync(int id)
        {
            UserRolesViewModel? model = null;
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                model = new UserRolesViewModel { UserId = id };
                if (user.Roles.Any(r => r.Name == "User")) model.IsUser = true;
                if (user.Roles.Any(r => r.Name == "Moderator")) model.IsModer = true;
                if (user.Roles.Any(r => r.Name == "Admin")) model.IsAdmin = true;
            }

            return model;
        }

        public async Task<List<Role>> GetRolesFromModelAsync(UserRolesViewModel model)
        {
            var roles = new List<Role>();
            Role? role = null;

            await SetRole(model.IsUser, "User");
            await SetRole(model.IsModer, "Moderator");
            await SetRole(model.IsAdmin, "Admin");

            async Task SetRole(bool checkRole, string nameRole)
            {
                if (checkRole)
                {
                    role = await _roleManager.FindByNameAsync(nameRole);
                    if (role != null) roles!.Add(role);
                }
            }

            return roles;
        }
    }
}
