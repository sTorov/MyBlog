using MyBlog.Services.ViewModels.Comments.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Comments.Request
{
    /// <summary>
    /// Модель создания комментария для API
    /// </summary>
    public class CommentApiCreateModel : ICommentCreateModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Добавьте текст комментария!")]
        public string Text { get; set; }
    }
}
