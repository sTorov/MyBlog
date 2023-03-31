using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Comments;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface ICommentService
    {
        Task<(User?, Post?)> CheckDataAtCreateComment(CommentController controller, CommentCreateViewModel model);
        Task CreateComment(User user, Post post, CommentCreateViewModel model);
        Task<CommentsViewModel> GetCommentsViewModel(int? postId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<CommentEditViewModel?> GetCommentEditViewModel(int id);
        Task<bool> UpdateComment(CommentEditViewModel model);
        Task<bool> DeleteComment(int id);
    }
}
