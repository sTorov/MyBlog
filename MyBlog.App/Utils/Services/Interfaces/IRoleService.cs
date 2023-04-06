using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Claim>> GetRoleClaims(User user);
    }
}
