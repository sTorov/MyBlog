using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Users.Response
{
    /// <summary>
    /// Модель представления регистрации пользователя
    /// </summary>
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailReg { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Никнейм")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(30, ErrorMessage = "{0} - Минимальная длина пароля: {1}, Максимальная: {2}", MinimumLength = 8)]
        public string PasswordReg { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
