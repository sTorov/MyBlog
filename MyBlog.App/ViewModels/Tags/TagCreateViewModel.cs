using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Tags
{
    public class TagCreateViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя тега")]
        public string Name { get; set; }
    }
}
