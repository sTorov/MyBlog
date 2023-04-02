using Microsoft.AspNetCore.Identity;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Data.DBModels.Users
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Photo { get; set; }

        public List<Role> Roles { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

        public User()
        {
            Photo = "https://thispersondoesnotexist.com/image";            
        }
    }
}
