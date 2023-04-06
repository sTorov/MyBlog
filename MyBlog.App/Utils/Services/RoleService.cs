using Microsoft.AspNetCore.Identity;
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

        public async Task<List<Claim>> GetRoleClaims(User user)
        {
            var claims = new List<Claim>();
            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));

            return claims;
        }
    }
}
