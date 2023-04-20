namespace MyBlog.App.ViewModels.Users
{
    public class UserCreateViewModel : UserRegisterViewModel
    {
        public Dictionary<string, bool>? AllRoles { get; set; }
    }
}
