using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.ViewModels.Posts.Interfaces;

namespace MyBlog.Services.Extensions
{
    /// <summary>
    /// Расширения статьи
    /// </summary>
    public static class PostExtensions
    {
        /// <summary>
        /// Присвоение значений модели редактирования сущности статьи
        /// </summary>
        public static Post Convert(this Post post, IPostRequestModel model)
        {
            post.Title = model.Title;
            post.Content = model.Content;

            return post;
        }
    }
}
