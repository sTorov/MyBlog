using Microsoft.AspNetCore.Identity;
using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Users;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IUserService
    {
        Task<(IdentityResult, User)> CreateUserAsync(UserRegisterViewModel model);
        Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess);
        Task<List<Claim>> GetClaimsAsync(User user);
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id, int? userId, bool fullAccess);
        Task<UsersViewModel?> GetUsersViewModelAsync(int? id);
        Task CheckDataAtRegistrationAsync(UserController controller, UserRegisterViewModel model);
        Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model);
        Task<User?> CheckDataAtEditionAsync(UserController controller, UserEditViewModel model);
        Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller);
    }
}
