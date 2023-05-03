using MyBlog.App.Controllers;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Tags.Interfaces;
using MyBlog.Services.ViewModels.Tags.Response;

namespace MyBlog.App.Utils.Modules
{
    /// <summary>
    /// Модель контроллера тегов
    /// </summary>
    public class TagControllerModule : ITagControllerModule
    {
        private readonly ITagService _tagService;

        public TagControllerModule(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<Tag?> CheckTagNameAsync<T>(TagController controller, T model) where T : ITagResponseViewModel
        {
            var checkTag = await _tagService.GetTagByNameAsync(model.Name);
            var check = model is TagEditViewModel editModel
                ? (checkTag != null && checkTag.Id != editModel.Id) : checkTag != null;

            if (check)
                controller.ModelState.AddModelError(string.Empty, $"Тег с именем [{model.Name}] уже существует!");

            return checkTag;
        }
    }
}
