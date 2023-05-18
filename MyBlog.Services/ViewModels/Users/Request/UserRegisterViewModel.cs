using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Users.Request
{
    /// <summary>
    /// Модель представления регистрации пользователя
    /// </summary>
    public class UserRegisterViewModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <example>FirstName</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        /// <example>SecondName</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        /// <example>LastName</example>
        [Display(Name = "Отчество")]
        public string? LastName { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        /// <example>example@gmail.com</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailReg { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Login</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Никнейм")]
        public string Login { get; set; }

        /// <summary>
        /// Дата рождения пользователя
        /// </summary>
        /// <example>2012-12-12</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        /// <example>11111111</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(30, ErrorMessage = "{0} - Минимальная длина пароля: {1}, Максимальная: {2}", MinimumLength = 8)]
        public string PasswordReg { get; set; }

        /// <summary>
        /// Повторный ввод пароля. Должен совпадать с PasswordReg
        /// </summary>
        /// <example>11111111</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
