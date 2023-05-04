using MyBlog.Data.DBModels.Users;
using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Data.DBModels.Comments
{
    /// <summary>
    /// Сущность комментария
    /// </summary>
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }        
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public Comment()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
