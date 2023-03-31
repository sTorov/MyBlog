using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface ICommentService
    {
        Task<(User?, Post?)> CheckDataAtCreateComment(CommentController controller, CommentCreateViewModel model);
        Task CreateComment(User user, Post post, CommentCreateViewModel model);
        Task<CommentsViewModel> GetCommentsViewModel(int? postId);
    }
}
