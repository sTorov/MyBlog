using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Roles;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetRolesByUserAsync(int userId) =>
            await _roleManager.Roles.Include(r => r.Users)
                .SelectMany(r => r.Users, (r, u) => new { Role = r, UserId = u.Id })
                .Where(o => o.UserId == userId).Select(o => o.Role).ToListAsync();
            
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

        public async Task<RolesViewModel?> GetRolesViewModelAsync(int? id)
        {
            var model = new RolesViewModel();
            if (id == null) 
                model.Roles = await _roleManager.Roles.ToListAsync();
            else 
            {
                var role = await _roleManager.FindByIdAsync(id.ToString() ?? "");
                if (role == null) return null;

                model.Roles = new List<Role> { role };
            }

            return model;
        }

        public async Task<bool> CreateRoleAsync(RoleCreateViewModel model)
        {
            var role = _mapper.Map<Role>(model);
            var result = await _roleManager.CreateAsync(role);

            return result.Succeeded;
        }

        public async Task<Role?> CheckDataForCreateTag(RoleController controller, RoleCreateViewModel model)
        {
            var checkRole = await _roleManager.FindByNameAsync(model.Name);
            if (checkRole != null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с именем [{model.Name}] уже существует!");
            return checkRole;
        }

        public async Task<Dictionary<string, bool>> GetEnabledRolesForUser(int id)
        {
            var dictionary = new Dictionary<string, bool>();
            var userRoles = await _roleManager.Roles.Include(r => r.Users)
                .SelectMany(r => r.Users, (r, u) => new { r.Name, u.Id }).Where(o => o.Id == id)
                .Select(i => i.Name).ToListAsync();

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var missRoles = allRoles.Except(userRoles);
            
            foreach(var role in missRoles)
            {
                if(role == null) continue;
                dictionary.Add(role, false);
            }
            foreach (var role in userRoles)
                dictionary.Add(role, true);

            return dictionary.OrderBy(p => p.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
