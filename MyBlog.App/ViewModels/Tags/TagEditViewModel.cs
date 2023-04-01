using System.ComponentModel.DataAnnotations;

namespace MyBlog.App.ViewModels.Tags
{
    public class TagEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя тега")]
        public string Name { get; set; }
    }
}
