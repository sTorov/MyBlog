using Microsoft.AspNetCore.Identity;
using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserRegisterViewModel model);
        Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int? id);
        Task<bool> DeleteByIdAsync(int id);
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id);
        Task CheckDataAtRegistration(UserController controller, UserRegisterViewModel model);
        Task CheckDataAtEdition(UserController controller, UserEditViewModel model, User currentUser);
    }
}
