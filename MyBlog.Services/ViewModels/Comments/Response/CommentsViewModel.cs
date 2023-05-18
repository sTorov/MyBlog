using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Services.ViewModels.Comments.Response
{
    /// <summary>
    /// Модель представления комментариев
    /// </summary>
    public class CommentsViewModel
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
