using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Data.DBModels.Posts
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public List<User> Users { get; set; }

        public Post()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
