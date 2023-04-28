using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services.ViewModels.Roles.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    public interface IRoleControllerModule
    {
        Task<Role?> CheckDataForCreateTagAsync(RoleController controller, RoleCreateViewModel model);
        Task<Role?> CheckDataAtEditAsync(RoleController controller, RoleEditViewModel model);
    }
}
