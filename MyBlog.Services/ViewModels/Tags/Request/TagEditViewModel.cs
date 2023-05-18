using MyBlog.Services.ViewModels.Attributes;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Tags.Request
{
    /// <summary>
    /// Модель представления редактирования тега
    /// </summary>
    public class TagEditViewModel : ITagUpdateModel
    {
        /// <summary>
        /// Идентификатор тега
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int Id { get; set; }

        /// <summary>
        /// Имя тега. Не должно содержать пробельных символов
        /// </summary>
        /// <example>tag_name</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [NotWhiteSpace(ErrorMessage = "Название тега не может содержать пробелов!")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
