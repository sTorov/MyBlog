using MyBlog.Services.ViewModels.Comments.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Comments.Request
{
    /// <summary>
    /// Модель обновления комментария для API
    /// </summary>
    public class CommentApiUpdateModel : ICommentEditModel
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        /// <example>100</example>
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public int Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        /// <example>text</example>
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public string Text { get; set; }
    }
}
