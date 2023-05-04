using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Services.Extensions
{
    /// <summary>
    /// Расширения роли
    /// </summary>
    public static class RoleExtensions
    {
        /// <summary>
        /// Присвоение значений модели редактирования сущности роли
        /// </summary>
        public static Role Convert(this Role role, RoleEditViewModel model)
        {
            role.Name = model.Name;
            role.Description = model.Description;

            return role;
        }
    }
}
