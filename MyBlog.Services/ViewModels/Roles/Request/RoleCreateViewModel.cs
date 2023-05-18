using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Roles.Request
{
    /// <summary>
    /// Модель представления создания роли
    /// </summary>
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
