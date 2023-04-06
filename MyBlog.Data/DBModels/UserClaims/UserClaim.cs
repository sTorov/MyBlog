using Microsoft.AspNetCore.Identity;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.DBModels.UserClaims
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public override int UserId { get; set; }
        public User User { get; set; }
    }
}
