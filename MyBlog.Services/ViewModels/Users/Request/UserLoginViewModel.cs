using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Users.Request
{
    /// <summary>
    /// Модель представления авторизации пользователя
    /// </summary>
    public class UserLoginViewModel
    {
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
