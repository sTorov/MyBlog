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
        /// Получение ролей пользователя
        /// </summary>
        Task<List<Role>> GetRolesByUserAsync(int userId);
        /// <summary>
        /// Получение роли по названию
        /// </summary>
        Task<Role?> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// Получение модели всех ролей
        /// </summary>
        Task<RolesViewModel?> GetRolesViewModelAsync(int? userId);
        /// <summary>
        /// Создание роли
        /// </summary>
        Task<bool> CreateRoleAsync(RoleCreateViewModel model);
        /// <summary>
        /// Получение списка всех ролей
        /// </summary>
        Task<List<Role>> GetAllRolesAsync();
        /// <summary>
        /// Получение словаря всех ролей. Ключ - название роли. Значение - наличие роли у пользователя
        /// </summary>
        Task<Dictionary<string, bool>> GetEnabledRolesForUserAsync(int id);
        /// <summary>
        /// Получение модели редактирования роли
        /// </summary>
        Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id);
        /// <summary>
        /// Обновление роли
        /// </summary>
        Task<bool> UpdateRoleAsync(Role role, RoleEditViewModel model);
        /// <summary>
        /// Удаление роли
        /// </summary>
        Task<bool> DeleteRoleAsync(int id);
        /// <summary>
        /// Получение модели указанной роли
        /// </summary>
        Task<RoleViewModel?> GetRoleViewModel(int id);
        /// <summary>
        /// Получение словаря ролей по умолчанию
        /// </summary>
        Task<Dictionary<string, bool>> GetDictionaryRolesDefault();
    }
}
