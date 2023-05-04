namespace MyBlog.Services.ViewModels.Users.Response
{
    /// <summary>
    /// Модель представления создания пользователя
    /// </summary>
    public class UserCreateViewModel : UserRegisterViewModel
    {
        public Dictionary<string, bool>? AllRoles { get; set; }
    }
}
