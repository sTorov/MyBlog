using MyBlog.Services.ViewModels.Comments.Response;
using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Services.Extensions
{
    /// <summary>
    /// Расширения для комментариев
    /// </summary>
    public static class CommentExtensions
    {
        /// <summary>
        /// Присвоение значений модели редактирования сущности комментария
        /// </summary>
        public static Comment Convert(this Comment comment, CommentEditViewModel model)
        {
            comment.Text = model.Text;
            comment.UserId = model.UserId;

            return comment;
        }
    }
}
