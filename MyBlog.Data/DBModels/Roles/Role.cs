using Microsoft.AspNetCore.Identity;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.DBModels.Roles
{
    /// <summary>
    /// Сущность роли
    /// </summary>
    public class Role : IdentityRole<int>
    {
        public List<User> Users { get; set; }
        public string? Description { get; set; }

        public Role(string name) : base(name) 
        {
            NormalizedName = name.ToUpper();
        }
    }
}
