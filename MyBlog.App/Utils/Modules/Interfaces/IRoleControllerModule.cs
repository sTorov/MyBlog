using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Roles.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    /// <summary>
    /// Интерфейс моделя для контроллера ролей
    /// </summary>
    public interface IRoleControllerModule
    {
        /// <summary>
        /// Проверка данных полученных контроллером при создании роли
        /// </summary>
        Task<Role?> CheckDataForCreateAsync(RoleController controller, RoleCreateViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании роли
        /// </summary>
        Task<Role?> CheckDataAtEditAsync(RoleController controller, RoleEditViewModel model);
    }
}
