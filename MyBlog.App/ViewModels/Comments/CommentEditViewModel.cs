using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Comments
{
    public class CommentEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Добавьте текст комментария!")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }
    }
}
