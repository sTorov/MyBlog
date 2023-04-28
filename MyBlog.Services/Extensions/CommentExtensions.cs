using MyBlog.Services.ViewModels.Comments.Response;
using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Services.Extensions
{
    public static class CommentExtensions
    {
        public static Comment Convert(this Comment comment, CommentEditViewModel model)
        {
            comment.Text = model.Text;
            comment.UserId = model.UserId;

            return comment;
        }
    }
}
