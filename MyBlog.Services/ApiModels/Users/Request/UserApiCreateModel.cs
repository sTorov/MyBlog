using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.Services.ApiModels.Users.Request
{
    /// <summary>
    /// Модель создания пользователя для API
    /// </summary>
    public class UserApiCreateModel : UserRegisterViewModel
    {
        public List<string> Roles { get; set; }
    }
}
