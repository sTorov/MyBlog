using MyBlog.Data.DBModels.Users;
using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Data.DBModels.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }        
        public DateTime DataCreated { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int RecipientId { get; set; }
        public Post Recipient { get; set; }
    }
}
