using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Roles;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesByUserAsync(int userId);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<RolesViewModel?> GetRolesViewModelAsync(int? userId);
        Task<bool> CreateRoleAsync(RoleCreateViewModel model);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> CheckDataForCreateTagAsync(RoleController controller, RoleCreateViewModel model);
        Task<Dictionary<string, bool>> GetEnabledRolesForUserAsync(int id);
        Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id);
        Task<Role?> CheckDataAtEditAsync(RoleController controller, RoleEditViewModel model);
        Task<bool> UpdateRoleAsync(Role role, RoleEditViewModel model);
        Task<bool> DeleteRoleAsync(int id);
        Task<RoleViewModel?> GetRoleViewModel(int id);
    }
}
