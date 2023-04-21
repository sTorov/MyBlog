using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.ViewModels.Users.Request
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
