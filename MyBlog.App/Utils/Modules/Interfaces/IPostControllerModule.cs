using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.ViewModels.Posts.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    /// <summary>
    /// Интерфейс модуля для контроллера статей
    /// </summary>
    public interface IPostControllerModule
    {
        /// <summary>
        /// Проверка данных полученных контроллером при обновлении статьи
        /// </summary>
        Task<Post?> CheckDataAtUpdateAsync(PostController controller, PostEditViewModel model);
    }
}
