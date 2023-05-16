using MyBlog.Services.ViewModels.Comments.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Comments.Response
{
    /// <summary>
    /// Модель представления создания комментария
    /// </summary>
    public class CommentCreateViewModel : ICommentEditModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
