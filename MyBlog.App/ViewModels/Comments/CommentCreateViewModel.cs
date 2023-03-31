using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Comments
{
    public class CommentCreateViewModel
    {
        [Required(ErrorMessage = "Добавьте текст комментария!")]
        [Display(Name = "Комментарий")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "PostId")]
        public int PostId { get; set; }
    }
}
