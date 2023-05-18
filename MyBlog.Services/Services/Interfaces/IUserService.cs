using Microsoft.AspNetCore.Identity;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ViewModels.Users.Intefaces;
using MyBlog.Data.DBModels.Roles;

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
        Task<(bool, User)> CreateUserAsync(UserRegisterViewModel model, List<Role>? roles = null);
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<bool> UpdateUserAsync(IUserUpdateModel model);
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
        /// Получение утверждений пользователя (роли, идентификатор)
        /// </summary>
        Task<List<Claim>> GetUserClaimsAsync(User user);
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
    }
}
