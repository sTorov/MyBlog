using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Comments.Response
{
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
