using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Services.ViewModels.Tags.Interfaces;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    public interface ITagControllerModule
    {
        Task<Tag?> CheckTagNameAsync<T>(TagController controller, T model) where T : ITagViewModel;
    }
}
