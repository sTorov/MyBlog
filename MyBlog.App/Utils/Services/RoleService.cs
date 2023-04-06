using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.App.Utils.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<Claim> GetRoleClaims(User user)
        {
            var claims = new List<Claim>();

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name!));

            return claims;
        }

        public async Task<List<Role>> GetRolesByUserAsync(int userId) =>
            await Task.Run(() => 
                _roleManager.Roles.Include(r => r.Users).AsEnumerable()
                .SelectMany(r => r.Users, (r, u) => new { Role = r, UserId = u.Id })
                .Where(o => o.UserId == userId).Select(o => o.Role).ToList()
            );
    }
}
