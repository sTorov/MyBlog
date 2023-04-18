using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Roles;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesByUserAsync(int userId);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<RolesViewModel?> GetRolesViewModelAsync(int? id);
        Task<bool> CreateRoleAsync(RoleCreateViewModel model);
        Task<Role?> CheckDataForCreateTag(RoleController controller, RoleCreateViewModel model);

        Task<UserRolesViewModel?> GetUserRolesViewModelAsync(int id);
        
        Task<List<Role>> GetRolesFromModelAsync(UserRolesViewModel model);
        Task<Dictionary<string, bool>> GetEnabledRolesForUser(int id);
    }
}
