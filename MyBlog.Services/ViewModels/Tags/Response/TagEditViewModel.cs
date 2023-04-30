using MyBlog.Services.ViewModels.Attributes;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Tags.Response
{
    public class TagEditViewModel : ITagResponseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [NotWhiteSpace(ErrorMessage = "Название тега не может содержать пробелов!")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
