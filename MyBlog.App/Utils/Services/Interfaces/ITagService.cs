using MyBlog.App.Controllers;
using MyBlog.App.ViewModels.Tags;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.Repositories;

namespace MyBlog.App.Utils.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagsViewModel?> GetTagsViewModelAsync(int? id);
        Task<Tag?> GetTagByIdAsync(int id);
        Task<List<Tag>> GetAllTags();
        Task<Tag?> CheckDataAtCreateTagAsync(TagController controller, TagCreateViewModel model);
        Task<Tag?> CheckDataAtEditTagAsync(TagController controller, TagEditViewModel model);
        Task CreateTagAsync(TagCreateViewModel model);
        Task<TagEditViewModel?> GetTagEditViewModelAsync(int id);
        Task<bool> UpdateTagAsync(TagEditViewModel model);
        Task<bool> DeleteTagAsync(int id);
    }
}
