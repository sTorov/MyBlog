using MyBlog.Data.DBModels.Users;

namespace MyBlog.Services.ViewModels.Users.Response
{
    /// <summary>
    /// Модель представления отображения всех пользователей
    /// </summary>
    public class UsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
