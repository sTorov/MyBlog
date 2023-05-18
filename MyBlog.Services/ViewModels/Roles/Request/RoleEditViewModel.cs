using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Roles.Request
{
    /// <summary>
    /// Модель представления редактирования роли
    /// </summary>
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
