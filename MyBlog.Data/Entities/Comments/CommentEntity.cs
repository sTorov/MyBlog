using MyBlog.Data.Entities.Users;
using MyBlog.Data.Entities.Posts;

namespace MyBlog.Data.Entities.Comments
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }        
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int PostId { get; set; }
        public PostEntity Post { get; set; }
    }
}
