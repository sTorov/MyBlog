using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Roles
{
    public class RoleEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
