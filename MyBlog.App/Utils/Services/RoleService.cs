using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Controllers;
using MyBlog.App.Utils.Extensions;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.App.ViewModels.Roles;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.App.Utils.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetRolesByUserAsync(int userId) =>
            await _roleManager.Roles.Include(r => r.Users)
                .SelectMany(r => r.Users, (r, u) => new { Role = r, UserId = u.Id })
                .Where(o => o.UserId == userId).Select(o => o.Role).ToListAsync();
            
        public async Task<Role?> GetRoleByNameAsync(string roleName) => await _roleManager.FindByNameAsync(roleName);

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

        public async Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id)
        {
            var checkRole = await _roleManager.FindByIdAsync(id.ToString());
            if (checkRole == null)
                return null;

            return _mapper.Map<RoleEditViewModel>(checkRole);
        }

        public async Task<Role?> CheckDataAtEditAsync(RoleController controller, RoleEditViewModel model)
        {
            var checkRole = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (checkRole == null)
                controller.ModelState.AddModelError(string.Empty, $"Роль с Id [{model.Id}] не найдена!");

            return checkRole;
        }

        public async Task<bool> UpdateRoleAsync(Role role, RoleEditViewModel model)
        {
            role.Convert(model);
            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}
