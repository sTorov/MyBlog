using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Roles.Request
{
    /// <summary>
    /// Модель представления создания роли
    /// </summary>
    public class RoleCreateViewModel
    {
        /// <summary>
        /// Имя роли
        /// </summary>
        /// <example>roleName</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        /// <summary>
        /// Описание роли
        /// </summary>
        /// <example>description</example>
        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
