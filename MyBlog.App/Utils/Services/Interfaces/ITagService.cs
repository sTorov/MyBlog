using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Tags;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagsViewModel?> GetTagsViewModelAsync(int? id);
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Post?> CheckDataAtCreateTagAsync(TagController controller, TagCreateViewModel model);
        Task<Tag?> CheckDataAtEditTagAsync(TagController controller, TagEditViewModel model);
        Task CreateTagAsync(TagCreateViewModel model, Post post);
        Task<TagEditViewModel?> GetTagEditViewModelAsync(int id);
        Task<bool> UpdateTagAsync(TagEditViewModel model);
        Task<bool> DeleteTagAsync(int id);
    }
}
