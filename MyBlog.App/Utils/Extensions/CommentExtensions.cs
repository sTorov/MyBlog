using MyBlog.App.ViewModels.Comments;
using MyBlog.Data.DBModels.Comments;

namespace MyBlog.App.Utils.Extensions
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
