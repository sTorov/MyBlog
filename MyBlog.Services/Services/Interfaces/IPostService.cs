using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Services.ViewModels.Posts.Request;
using MyBlog.Data.DBModels.Posts;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.ViewModels.Posts.Interfaces;

namespace MyBlog.Services.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервисов сущности статьи
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Создание статьи
        /// </summary>
        Task<bool> CreatePostAsync(IPostCreateModel model);
        /// <summary>
        /// Получение модели всез статей
        /// </summary>
        Task<PostsViewModel> GetPostsViewModelAsync(int? tagId, int? userId);
        /// <summary>
        /// Получение модели указаной статьи
        /// </summary>
        Task<PostViewModel?> GetPostViewModelAsync(int id, string userId);
        /// <summary>
        /// Получение модели редактирования статьи
        /// </summary>
        Task<(PostEditViewModel?, IActionResult?)> GetPostEditViewModelAsync(int id, string? userId, bool fullAccess);
        /// <summary>
        /// Удаление статьи (основное приложение)
        /// </summary>
        Task<(IActionResult?, bool)> DeletePostAsync(int id, int userId, bool fullAccess);
        /// <summary>
        /// Удаление статьи (API)
        /// </summary>
        Task<int> DeletePostAsync(Post post);
        /// <summary>
        /// Получение статьи по идентификатору
        /// </summary>
        Task<Post?> GetPostByIdAsync(int id);
        /// <summary>
        /// Получение списка всех статей
        /// </summary>
        Task<List<Post>> GetAllPostsAsync();
        /// <summary>
        /// Получение статей определённого пользователя
        /// </summary>
        Task<List<Post>?> GetPostsByUserIdAsync(int userId);
        /// <summary>
        /// Получение статей с определённым тегом
        /// </summary>
        Task<List<Post>?> GetPostsByTagIdAsync(int tagId);
        /// <summary>
        /// Обновление статьи
        /// </summary>
        Task<bool> UpdatePostAsync(IPostUpdateModel model);
        /// <summary>
        /// Получение идентификатора последней созданой статьи указанного пользователя
        /// </summary>
        Task<int> GetLastCreatePostIdByUserId(int userId);
    }
}
