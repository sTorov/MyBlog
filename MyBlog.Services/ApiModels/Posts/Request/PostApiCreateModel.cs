using MyBlog.Services.ViewModels.Posts.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Posts.Request
{
    /// <summary>
    /// Модель обновления статьи для API
    /// </summary>
    public class PostApiCreateModel : IPostCreateModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int UserId { get; set; }

        /// <summary>
        /// Список тегов. Указывать через пробел. Имена тегов не могут содержать пробелов.
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
