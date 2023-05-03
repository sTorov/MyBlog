using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Services.ViewModels.Tags.Interfaces;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    /// <summary>
    /// Интерфейс модуля для контроллера тегов
    /// </summary>
    public interface ITagControllerModule
    {
        /// <summary>
        /// Проверка данных о теге, полученных контроллером
        /// </summary>
        Task<Tag?> CheckTagNameAsync<T>(TagController controller, T model) where T : ITagResponseViewModel;
    }
}
