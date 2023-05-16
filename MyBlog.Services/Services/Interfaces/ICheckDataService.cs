using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.ViewModels.Posts.Response;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Services.ApiModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Intefaces;
using MyBlog.Services.ApiModels.Roles.Request;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов проверки данных
    /// </summary>
    public interface ICheckDataService
    {
        /// <summary>
        /// Проверка сущностей по идентификаторам
        /// </summary>
        Task<List<string>> CheckEntitiesByIdAsync(int? postId = null, int? userId = null, int? roleId = null, int? tagId = null, int? commentId = null);

        /// <summary>
        /// Проверка сущностей по имени
        /// </summary>
        Task<List<string>> CheckEntitiesByNameAsync(string? userName = null, string? roleName = null, string? tagName = null);


        /// <summary>
        /// Проверка данных полученных контроллером при обновлении статьи
        /// </summary>
        Task CheckDataForUpdatePostAsync(Controller controller, PostEditViewModel model);


        /// <summary>
        /// Проверка данных полученных контроллером при создании роли
        /// </summary>
        Task CheckDataForCreateRoleAsync(Controller controller, RoleCreateViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании роли
        /// </summary>
        Task CheckDataForEditRoleAsync(Controller controller, RoleEditViewModel model);
        /// <summary>
        /// Проверка изменения стандартных ролей приложения
        /// </summary>
        Task<bool> CheckChangeDefaultRolesAsync(int roleId, string roleName = "");


        /// <summary>
        /// Проверка данных о теге, полученных контроллером (основное приложение)
        /// </summary>
        Task CheckTagNameAsync<T>(Controller controller, T model) where T : ITagResponseViewModel;
        /// <summary>
        /// Проверка данных о теге, полученных контроллером (API)
        /// </summary>
        Task<string> CheckTagNameAsync<T>(T model) where T : ITagResponseViewModel;
        /// <summary>
        /// Проверка тегов на существование при создании статьи
        /// </summary>
        Task<bool> CheckTagsForCreatePostAsync(string tags);


        /// <summary>
        /// Проверка данных полученных контроллером при создании пользователя (основное приложение)
        /// </summary>
        Task CheckDataForCreateUserAsync(Controller controller, UserRegisterViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при создании пользователя (API)
        /// </summary>
        Task<List<string>> CheckDataForCreateUserAsync(UserRegisterViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при авторизации пользователя
        /// </summary>
        Task<User?> CheckDataForLoginAsync(Controller controller, UserLoginViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании пользователя (основное приложение)
        /// </summary>
        Task<User?> CheckDataForEditUserAsync(Controller controller, UserEditViewModel model);
        /// <summary>
        /// Проверка данных полученных контроллером при редактировании пользователя (API)
        /// </summary>
        Task<(User?, List<string>)> CheckDataForEditUserAsync(IUserUpdateModel model);
        /// <summary>
        /// Проверка корректности переданных ролей в модели обновления пользователя
        /// </summary>
        Task<bool> CheckRolesForUserUpdateModel(IUserUpdateModel model);
    }
}
