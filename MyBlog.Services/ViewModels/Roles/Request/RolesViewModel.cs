using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Services.ViewModels.Roles.Request
{
    public class RolesViewModel
    {
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
