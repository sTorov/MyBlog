using MyBlog.Data.DBModels.Tags;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Posts.Response
{
    public class PostCreateViewModel
    {
        public int UserId { get; set; }

        public List<Tag> AllTags { get; set; }

        public string? PostTags { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Добавьте контент!")]
        [Display(Name = "Контент")]
        public string Content { get; set; }
    }
}
