using MyBlog.Services.ViewModels.Users.Request;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Users.Request
{
    /// <summary>
    /// Модель создания пользователя для API
    /// </summary>
    public class UserApiCreateModel : UserRegisterViewModel
    {
        /// <summary>
        /// Роли пользователя. При заполнении обязательно должна приутствовать роль User
        /// </summary>
        /// <example>["User", "Admin"]</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public List<string> Roles { get; set; }
    }
}
