using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Comments.Response
{
    /// <summary>
    /// Модель представления редактирования комментария
    /// </summary>
    public class CommentEditViewModel
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessage = "Добавьте текст комментария!")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
