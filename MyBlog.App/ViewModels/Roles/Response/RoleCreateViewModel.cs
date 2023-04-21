using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Roles.Response
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
