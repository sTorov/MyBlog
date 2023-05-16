﻿using MyBlog.Services.ViewModels.Posts.Request;
using MyBlog.Services.ViewModels.Posts.Response;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
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
        Task<bool> CreatePostAsync(IPostCreateModel model, List<Tag>? tags);
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
        /// Удаление статьи
        /// </summary>
        Task<(IActionResult?, bool)> DeletePostAsync(int id, int userId, bool fullAccess);
        /// <summary>
        /// Получение статьи по идентификатору
        /// </summary>
        Task<Post?> GetPostByIdAsync(int id);
        /// <summary>
        /// Обновление статьи
        /// </summary>
        Task<bool> UpdatePostAsync(IPostResponceModel model, Post post);
        /// <summary>
        /// Получение идентификатора последней созданой статьи указанного пользователя
        /// </summary>
        Task<int> GetLastCreatePostIdByUserId(int userId);
    }
}
