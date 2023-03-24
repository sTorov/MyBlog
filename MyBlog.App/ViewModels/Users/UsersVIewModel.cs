using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.ViewModels.Users
{
    public class UsersVIewModel
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
