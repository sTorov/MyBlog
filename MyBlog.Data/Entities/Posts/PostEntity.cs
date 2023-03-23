using MyBlog.Data.Entities.Tags;
using MyBlog.Data.Entities.Users;

namespace MyBlog.Data.Entities.Posts
{
    public class PostEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public List<TagEntity> Tags { get; set; }
    }
}
