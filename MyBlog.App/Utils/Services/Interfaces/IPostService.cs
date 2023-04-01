using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IPostService
    {
        Task<User?> CheckDataAtCreated(PostController controller, PostCreateViewModel model);
        Task CreatePost(User user, PostCreateViewModel model);
        Task<PostsViewModel> GetPostViewModel(int? userId);
        Task<PostEditViewModel?> GetPostEditViewModel(int id);
        Task<bool> DeletePost(int id);
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> UpdatePostAsync(PostEditViewModel model);
    }
}
