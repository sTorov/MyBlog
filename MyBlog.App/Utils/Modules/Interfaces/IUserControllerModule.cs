using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    /// <summary>
    /// Интерфейс модуля для контроллера пользователей
    /// </summary>
    public interface IUserControllerModule
    {
        /// <summary>
        /// Проверка данных полученных контроллером при создании пользователя
        /// </summary>
        Task CheckDataAtCreationAsync(UserController controller, UserRegisterViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при авторизации пользователя
        /// </summary>
        Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании пользователя
        /// </summary>
        Task<User?> CheckDataAtEditionAsync(UserController controller, UserEditViewModel model);
        /// <summary>
        /// Получение данных об обновлении ролей пользователя
        /// </summary>
        Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller);
    }
}
