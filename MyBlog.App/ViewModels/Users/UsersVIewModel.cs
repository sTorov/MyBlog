using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.ViewModels.Users
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
