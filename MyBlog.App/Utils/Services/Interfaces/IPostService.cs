using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostCreateViewModel model, List<Tag>? tags);
        Task<PostsViewModel> GetPostsViewModelAsync(int? userId);
        Task<PostViewModel?> GetPostViewModelAsync(int id);
        Task<PostEditViewModel?> GetPostEditViewModelAsync(int id, int? userId, bool fullAccess);
        Task<bool> DeletePostAsync(int id, int userId, bool fullAccess);
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> UpdatePostAsync(PostEditViewModel model, Post post);
        Task<Post?> CheckDataAtUpdatePostAsync(PostController controller, PostEditViewModel model);
    }
}
