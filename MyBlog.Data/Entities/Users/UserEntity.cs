using Microsoft.AspNetCore.Identity;
using MyBlog.Data.Entities.Comments;
using MyBlog.Data.Entities.Posts;
using System.Runtime.CompilerServices;

namespace MyBlog.Data.Entities.Users
{
    public class UserEntity : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BitrhDate { get; set; }
        public string Photo { get; set; }

        public List<PostEntity> Posts { get; set; }
        public List<CommentEntity> Comments { get; set; }
    }
}
