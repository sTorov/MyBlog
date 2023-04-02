using Microsoft.AspNetCore.Identity;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.DBModels.Roles
{
    public class Role : IdentityRole<int>
    {
        public List<User> Users { get; set; }
        
        public Role(string name) : base(name) { }
    }
}
