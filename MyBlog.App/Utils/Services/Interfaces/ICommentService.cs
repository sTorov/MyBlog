using Microsoft.AspNetCore.Mvc;
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
        Task<IActionResult?> CheckDataAtCreateComment(CommentController controller);
        Task<bool> CreateComment(CommentCreateViewModel model);
        Task<CommentsViewModel> GetCommentsViewModel(int? postId, string? userId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<CommentEditViewModel?> GetCommentEditViewModel(int id);
        Task<bool> UpdateComment(CommentEditViewModel model);
        Task<bool> DeleteComment(int id);
    }
}
