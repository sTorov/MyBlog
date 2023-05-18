using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Services.ViewModels.Roles.Response
{
    /// <summary>
    /// Модель представления всех ролей
    /// </summary>
    public class RolesViewModel
    {
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
