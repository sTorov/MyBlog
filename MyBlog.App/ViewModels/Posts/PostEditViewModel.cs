using MyBlog.Data.DBModels.Tags;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Posts
{
    public class PostEditViewModel
    {
        public int Id { get; set; }

        public List<Tag>? AllTags { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Тэги (указать через пробел)")]
        public string? PostTags { get; set; }

        [Required(ErrorMessage = "Добавьте контент!")]
        [Display(Name = "Контент")]
        public string Content { get; set; }
    }
}
