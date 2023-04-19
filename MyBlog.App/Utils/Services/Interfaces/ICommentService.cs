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
        Task<bool> CreateCommentAsync(CommentCreateViewModel model);
        Task<CommentsViewModel> GetCommentsViewModelAsync(int? postId, int? userId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetAllCommentsByPostIdAsync(int postId);
        Task<CommentEditViewModel?> GetCommentEditViewModelAsync(int id, int? userId, bool fullAccess);
        Task<bool> UpdateCommentAsync(CommentEditViewModel model);
        Task<bool> DeleteCommentAsync(int id, int? userId, bool fullAccess);
    }
}
