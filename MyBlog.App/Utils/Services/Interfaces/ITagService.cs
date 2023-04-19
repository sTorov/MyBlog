using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Tags;
using MyBlog.App.ViewModels.Tags.Interfaces;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagsViewModel?> GetTagsViewModelAsync(int? tagId, int? postId);
        Task<Tag?> GetTagByIdAsync(int id);
        Task<List<Tag>> GetAllTagsAsync();
        Task<Tag?> CheckTagNameAsync<T>(TagController controller, T model) where T : ITagViewModel;
        Task<bool> CreateTagAsync(TagCreateViewModel model);
        Task<List<Tag>?> CreateTagForPostAsync(string? postTags);
        Task<TagEditViewModel?> GetTagEditViewModelAsync(int id);
        Task<bool> UpdateTagAsync(TagEditViewModel model);
        Task<bool> DeleteTagAsync(int id);
    }
}
