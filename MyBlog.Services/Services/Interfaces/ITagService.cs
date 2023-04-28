using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Services.ViewModels.Tags.Request;
using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Services.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagsViewModel?> GetTagsViewModelAsync(int? tagId, int? postId);
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Tag?> GetTagByNameAsync(string name);
        Task<List<Tag>> GetAllTagsAsync();
        Task<bool> CreateTagAsync(TagCreateViewModel model);
        Task<List<Tag>?> CreateTagForPostAsync(string? postTags);
        Task<TagEditViewModel?> GetTagEditViewModelAsync(int id);
        Task<bool> UpdateTagAsync(TagEditViewModel model);
        Task<bool> DeleteTagAsync(int id);
        Task<TagViewModel?> GetTagViewModelAsync(int id);
    }
}
