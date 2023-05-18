using MyBlog.Services.ViewModels.Posts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Posts.Request
{
    /// <summary>
    /// Модель создания статьи для API
    /// </summary>
    public class PostApiUpdateModel : IPostUpdateModel
    {
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        /// <example>11</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int Id { get; set; }

        /// <summary>
        /// Список тегов. Указывать через пробел. Названия тегов не могут содержать пробелов
        /// </summary>
        /// <example>"tag1 tag2"</example>
        public string? PostTags { get; set; }

        /// <summary>
        /// Заголовок статьи
        /// </summary>
        /// <example>title</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string Title { get; set; }

        /// <summary>
        /// Содержание статьи
        /// </summary>
        /// <example>content</example>
        [Required(ErrorMessage = "Добавьте контент!")]
        public string Content { get; set; }
    }
}
