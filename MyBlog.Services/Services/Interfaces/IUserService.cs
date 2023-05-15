using Microsoft.AspNetCore.Identity;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ViewModels.Users.Intefaces;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов сущности пользователя
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<(IdentityResult, User)> CreateUserAsync(UserRegisterViewModel model);
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<IdentityResult> CreateUserAsync(UserCreateViewModel model);
        /// <summary>
        /// Обновление пользователя
        /// </summary>
        Task<IdentityResult> UpdateUserAsync(IUserUpdateModel model, User user);
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        Task<List<User>> GetAllUsersAsync();
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);
        /// <summary>
        /// Получение пользователя по почтовому адресу
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);
        /// <summary>
        /// Получение пользователя по имени
        /// </summary>
        Task<User?> GetUserByNameAsync(string name);
        /// <summary>
        /// Удаление пользователя (основное приложение)
        /// </summary>
        Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess);
        /// <summary>
        /// Удаление пользователя (API)
        /// </summary>
        Task<bool> DeleteByIdAsync(User user);
        /// <summary>
        /// Получение утверждений пользователя
        /// </summary>
        Task<List<Claim>> GetClaimsAsync(User user);
        /// <summary>
        /// Получение модели редактирования пользователя
        /// </summary>
        Task<(UserEditViewModel?, IActionResult?)> GetUserEditViewModelAsync(int id, string? userId, bool fullAccess);
        /// <summary>
        /// Получение модели всех пользователей
        /// </summary>
        Task<UsersViewModel?> GetUsersViewModelAsync(int? roleId);
        /// <summary>
        /// Получение модели профиля пользователя
        /// </summary>
        Task<UserViewModel?> GetUserViewModelAsync(int id);
        /// <summary>
        /// Получение данных об обновлении ролей пользователя
        /// </summary>
        Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(HttpRequest request);
    }
}
