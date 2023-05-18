using MyBlog.Services.ViewModels.Users.Intefaces;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ApiModels.Users.Request
{
    /// <summary>
    /// Модель обновления пользователя для API
    /// </summary>
    public class UserApiUpdateModel : IUserUpdateModel
    {
        public int Id { get; set; }

        public List<string> Roles { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string SecondName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }
    }
}
