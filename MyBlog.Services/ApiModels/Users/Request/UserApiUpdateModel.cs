using MyBlog.Services.ViewModels.Users.Intefaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Users.Request
{
    /// <summary>
    /// Модель обновления пользователя для API
    /// </summary>
    public class UserApiUpdateModel : IUserUpdateModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        /// <example>123</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int Id { get; set; }

        /// <summary>
        /// Список ролей. При заполнении обязательно должна присутствовать роль User
        /// </summary>
        /// <example>["User", "Admin"]</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <example>FirstName</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        /// <example>SecondName</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        /// <example>LastName</example>
        public string? LastName { get; set; }

        /// <summary>
        /// Почтовый адрес пользователя
        /// </summary>
        /// <example>example@gmail.com</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Дата рождения пользователя
        /// </summary>
        /// <example>2012-12-12</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Login</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string Login { get; set; }

        /// <summary>
        /// Ссылка на изображение, используемое в качестве аватара
        /// </summary>
        /// <example>https://example.com</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }
    }
}
