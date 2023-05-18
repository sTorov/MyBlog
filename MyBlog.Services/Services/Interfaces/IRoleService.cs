using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Services.ViewModels.Roles.Request;
using MyBlog.Data.DBModels.Roles;
using Microsoft.AspNetCore.Http;

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
        /// Получение роли по идентификатору
        /// </summary>
        Task<Role?> GetRoleByIdAsync(int id);
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
        /// Получение модели редактирования роли
        /// </summary>
        Task<RoleEditViewModel?> GetRoleEditViewModelAsync(int id);
        /// <summary>
        /// Обновление роли
        /// </summary>
        Task<bool> UpdateRoleAsync(RoleEditViewModel model);
        /// <summary>
        /// Удаление роли
        /// </summary>
        Task<bool> DeleteRoleAsync(int id);
        /// <summary>
        /// Получение модели указанной роли
        /// </summary>
        Task<RoleViewModel?> GetRoleViewModel(int id);
        /// <summary>
        /// Получение данных об обновлении ролей пользователя
        /// </summary>
        List<string> GetEnabledRoleNamesWithRequest(HttpRequest request);
        /// <summary>
        /// Преобразование списка имён ролей в список ролей 
        /// </summary>
        Task<List<Role>> ConvertRoleNamesInRoles(List<string> roleNames);
    }
}
