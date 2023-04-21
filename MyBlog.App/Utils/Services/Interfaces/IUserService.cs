using Microsoft.AspNetCore.Identity;
using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Users.Request;
using MyBlog.App.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IUserService
    {
        Task<(IdentityResult, User)> CreateUserAsync(UserRegisterViewModel model);
        Task<IdentityResult> CreateUserAsync(UserCreateViewModel model);
        Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess);
        Task<List<Claim>> GetClaimsAsync(User user);
        Task<UserEditViewModel?> GetUserEditViewModelAsync(int id, int? userId, bool fullAccess);
        Task<UsersViewModel?> GetUsersViewModelAsync(int? roleId);
        Task CheckDataAtCreationAsync(UserController controller, UserRegisterViewModel model);
        Task<User?> CheckDataAtLoginAsync(UserController controller, UserLoginViewModel model);
        Task<User?> CheckDataAtEditionAsync(UserController controller, UserEditViewModel model);
        Task<Dictionary<string, bool>> UpdateRoleStateForEditUserAsync(UserController controller);
        Task<UserViewModel?> GetUserViewModelAsync(int id);
    }
}
