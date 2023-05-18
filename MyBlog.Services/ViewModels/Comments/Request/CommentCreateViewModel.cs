using MyBlog.Services.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Comments.Request
{
    /// <summary>
    /// Модель представления создания комментария
    /// </summary>
    public class CommentCreateViewModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

        [Required(ErrorMessage = "Добавьте текст комментария!")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
