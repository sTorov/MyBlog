using MyBlog.Services.ViewModels.Attributes;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Tags.Request
{
    /// <summary>
    /// Модель представления создания тега
    /// </summary>
    public class TagCreateViewModel : ITagRequestViewModel
    {
        /// <summary>
        /// Имя тега. Не должно содержать пробельные символы
        /// </summary>
        /// <example>tag_name</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [NotWhiteSpace(ErrorMessage = "Название тега не может содержать пробелов!")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
