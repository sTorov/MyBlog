using Microsoft.AspNetCore.Identity;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.Services.Services.Interfaces
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
        Task<UserViewModel?> GetUserViewModelAsync(int id);
    }
}
