using MyBlog.Services.ViewModels.Roles.Request;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов сущности роли
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 
        /// </summary>
        Task<List<Role>> GetRolesByUserAsync(int userId);
        /// <summary>
        /// 
        /// </summary>
        Task<Role?> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// 
        /// </summary>
        Task<RolesViewModel?> GetRolesViewModelAsync(int? userId);
        /// <summary>
        /// 
        /// </summary>
        Task<bool> CreateRoleAsync(RoleCreateViewModel model);
        /// <summary>
        /// 
        /// </summary>
        Task<List<Role>> GetAllRolesAsync();
        /// <summary>
        /// 
        /// </summary>
        Task<Dictionary<string, bool>> GetEnabledRolesForUserAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        Task<bool> UpdateRoleAsync(Role role, RoleEditViewModel model);
        /// <summary>
        /// 
        /// </summary>
        Task<bool> DeleteRoleAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        Task<RoleViewModel?> GetRoleViewModel(int id);
        /// <summary>
        /// 
        /// </summary>
        Task<Dictionary<string, bool>> GetDictionaryRolesDefault();
    }
}
