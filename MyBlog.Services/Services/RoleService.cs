using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Services.Extensions;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Roles.Request;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using Microsoft.AspNetCore.Http;

namespace MyBlog.Services.Services
{
    /// <summary>
    /// Сервисы сущности роли
    /// </summary>
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

        public async Task<Role?> GetRoleByIdAsync(int id) => await _roleManager.FindByIdAsync(id.ToString());

        public async Task<RolesViewModel?> GetRolesViewModelAsync(int? userId)
        {
            var model = new RolesViewModel();
            if (userId == null) 
                model.Roles = await _roleManager.Roles.ToListAsync();
            else 
            {
                var user = await _userManager.Users.Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return null;

                model.Roles = user.Roles;
            }

            return model;
        }

        public async Task<bool> CreateRoleAsync(RoleCreateViewModel model)
        {
            var role = _mapper.Map<Role>(model);
            var result = await _roleManager.CreateAsync(role);

            return result.Succeeded;
        }

        public async Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id)
        {
            var checkRole = await _roleManager.FindByIdAsync(id.ToString());
            if (checkRole == null)
                return null;

            return _mapper.Map<RoleEditViewModel>(checkRole);
        }

        public async Task<List<Role>> GetAllRolesAsync() => await _roleManager.Roles.ToListAsync();

        public async Task<bool> UpdateRoleAsync(RoleEditViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null) return false;

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

        public async Task<RoleViewModel?> GetRoleViewModel(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return null;

            return _mapper.Map<RoleViewModel>(role);
        }

        public List<string> GetEnabledRoleNamesWithRequest(HttpRequest request)
        {
            var list = new List<string>();

            foreach (var pair in request.Form)
            {
                if (pair.Value == "on")
                    list.Add(pair.Key);
            }

            return list;
        }

        public async Task<List<Role>> ConvertRoleNamesInRoles(List<string> roleNames)
        {
            var list = new List<Role>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                    list.Add(role);
            }

            return list;
        }
    }
}
