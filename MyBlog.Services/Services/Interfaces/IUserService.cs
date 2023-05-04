﻿using Microsoft.AspNetCore.Identity;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

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
        Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user);
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
        /// Удаление пользователя
        /// </summary>
        Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess);
        /// <summary>
        /// Получение утверждений пользователя
        /// </summary>
        Task<List<Claim>> GetClaimsAsync(User user);
        /// <summary>
        /// Получение модели редактирования пользователя
        /// </summary>
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id, int? userId, bool fullAccess);
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
