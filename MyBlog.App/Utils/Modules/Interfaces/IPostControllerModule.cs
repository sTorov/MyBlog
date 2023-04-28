using MyBlog.App.Controllers;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Services.ViewModels.Posts.Response;

namespace MyBlog.App.Utils.Modules.Interfaces
{
    public interface IPostControllerModule
    {
        Task<Post?> CheckDataAtUpdatePostAsync(PostController controller, PostEditViewModel model);
    }
}
