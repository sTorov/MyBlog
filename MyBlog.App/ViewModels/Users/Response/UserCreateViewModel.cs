namespace MyBlog.App.ViewModels.Users.Response
{
    public class UserCreateViewModel : UserRegisterViewModel
    {
        public Dictionary<string, bool>? AllRoles { get; set; }
    }
}
