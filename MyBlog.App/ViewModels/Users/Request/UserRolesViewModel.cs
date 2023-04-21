using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Users.Request
{
    public class UserRolesViewModel
    {
        public int UserId { get; init; }

        [Display(Name = "Пользователь")]
        public bool IsUser { get; set; }

        [Display(Name = "Модератор")]
        public bool IsModer { get; set; }

        [Display(Name = "Админ")]
        public bool IsAdmin { get; set; }

    }
}
