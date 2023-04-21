using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyBlog.App.ViewModels.Users.Response
{
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
