using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Services.ViewModels.Comments.Request
{
    public class CommentsViewModel
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
