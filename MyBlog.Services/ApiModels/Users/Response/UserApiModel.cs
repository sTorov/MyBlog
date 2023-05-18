using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyBlog.Services.ApiModels.Users.Response
{
    /// <summary>
    /// Модель пользователя для API
    /// </summary>
    public class UserApiModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>123</example>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <example>FirstName</example>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        /// <example>SecondName</example>
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        /// <example>LastName</example>
        public string? LastName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Login</example>
        public string Login { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        /// <example>examole@gmail.com</example>
        public string Email { get; set; }

        /// <summary>
        /// День рождения пользователя
        /// </summary>
        /// <example>12.12.2000</example>
        public string BirthDate { get; set; }

        /// <summary>
        /// Ссылка на изображение, используемое в качестве аватара
        /// </summary>
        /// <example>https://example.com</example>
        public string Photo { get; set; }

        /// <summary>
        /// Список ролей
        /// </summary>
        ///<example>["User", "Admin"]</example>
        public List<string> Roles { get; set; }
    }
}
