using MyBlog.Services.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Comments.Request
{
    /// <summary>
    /// Модель представления создания комментария
    /// </summary>
    public class CommentCreateViewModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        /// <example>11</example>
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public int PostId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        /// <example>text</example>
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
