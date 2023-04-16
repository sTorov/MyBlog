using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Posts;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface IPostService
    {
        Task<bool> CreatePost(PostCreateViewModel model, List<Tag>? tags);
        Task<PostsViewModel> GetPostViewModel(int? postId, string? userId);
        Task<PostEditViewModel?> GetPostEditViewModel(int id, string? userId);
        Task<bool> DeletePost(int id);
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> UpdatePostAsync(PostEditViewModel model);
    }
}
