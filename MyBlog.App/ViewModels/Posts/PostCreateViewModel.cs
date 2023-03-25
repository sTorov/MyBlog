using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Posts
{
    public class PostCreateViewModel
    {
        [Display(Name = "UserId")]
        public int UsertId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Добавьте контент!")]
        [Display(Name = "Контент")]
        public string Content { get; set; }
    }
}
