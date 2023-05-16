using MyBlog.Services.ViewModels.Comments.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Comments.Request
{
    /// <summary>
    /// Модель обновления комментария для API
    /// </summary>
    public class CommentApiUpdateModel : ICommentEditModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public string Text { get; set; }
    }
}
