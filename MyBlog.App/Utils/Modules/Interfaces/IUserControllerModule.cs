using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Users;
using MyBlog.Services.ViewModels.Users.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    public interface IUserControllerModule
    {
        Task CheckDataAtCreationAsync(UserController controller, UserRegisterViewModel model);
        Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model);
        Task<User?> CheckDataAtEditionAsync(UserController controller, UserEditViewModel model);
        Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller);
    }
}
