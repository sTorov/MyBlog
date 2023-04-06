using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IRoleService
    {
        List<Claim> GetRoleClaims(User user);
        Task<List<Role>> GetRolesByUserAsync(int userId);
    }
}
