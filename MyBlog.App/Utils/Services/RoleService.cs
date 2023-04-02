using Microsoft.AspNetCore.Identity;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.App.Utils.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
    }
}
