using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.ViewModels.Posts.Response;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов проверки данных
    /// </summary>
    public interface ICheckDataService
    {
        /// <summary>
        /// Проверка данных полученных контроллером при обновлении статьи
        /// </summary>
        Task<Post?> CheckDataForUpdatePostAsync(Controller controller, PostEditViewModel model);


        /// <summary>
        /// Проверка данных полученных контроллером при создании роли
        /// </summary>
        Task<Role?> CheckDataForCreateRoleAsync(Controller controller, RoleCreateViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании роли
        /// </summary>
        Task<Role?> CheckDataForEditRoleAsync(Controller controller, RoleEditViewModel model);


        /// <summary>
        /// Проверка данных о теге, полученных контроллером
        /// </summary>
        Task<Tag?> CheckTagNameAsync<T>(Controller controller, T model) where T : ITagResponseViewModel;


        /// <summary>
        /// Проверка данных полученных контроллером при создании пользователя
        /// </summary>
        Task CheckDataForCreateUserAsync(Controller controller, UserRegisterViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при авторизации пользователя
        /// </summary>
        Task<User?> CheckDataForLoginAsync(Controller controller, UserLoginViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании пользователя
        /// </summary>
        Task<User?> CheckDataForEditUserAsync(Controller controller, UserEditViewModel model);
    }
}
