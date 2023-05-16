using MyBlog.Services.ViewModels.Comments.Request;
using MyBlog.Services.ViewModels.Comments.Response;
using MyBlog.Data.DBModels.Comments;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ViewModels.Comments.Interfaces;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов сущности комментария
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Создание комментария
        /// </summary>
        Task<bool> CreateCommentAsync(CommentCreateViewModel model);
        /// <summary>
        /// Получение модели всех комментариев
        /// </summary>
        Task<CommentsViewModel> GetCommentsViewModelAsync(int? postId, int? userId);
        /// <summary>
        /// Получение комментария по идентификатору
        /// </summary>
        Task<Comment?> GetCommentByIdAsync(int id);
        /// <summary>
        /// Получение всех комментариев для указанной статьи
        /// </summary>
        Task<List<Comment>> GetAllCommentsByPostIdAsync(int postId);
        /// <summary>
        /// Получение модели редактирования комментария
        /// </summary>
        Task<(CommentEditViewModel?, IActionResult?)> GetCommentEditViewModelAsync(int id, string? userId, bool fullAccess);
        /// <summary>
        /// Обновление комментария
        /// </summary>
        Task<bool> UpdateCommentAsync(ICommentEditModel model);
        /// <summary>
        /// Удаление комментария
        /// </summary>
        Task<(IActionResult?, bool)> DeleteCommentAsync(int id, int? userId, bool fullAccess);
    }
}
