using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Users.Intefaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Users.Response
{
    /// <summary>
    /// Модель представления редактирования пользователя
    /// </summary>
    public class UserEditViewModel : IUserUpdateModel
    {
        public int Id { get; set; }

        public string? ReturnUrl { get; set; } 

        public List<string>? Roles { get; set; }

        public List<string>? AllRoles { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Display(Name = "Никнейм")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Ссылка на изображение")]
        public string Photo { get; set; }
    }
}
