using MyBlog.Services.ViewModels.Tags.Request;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Services.Extensions
{
    /// <summary>
    /// Расширения тега
    /// </summary>
    public static class TagExtensions
    {
        /// <summary>
        /// Присвоение значений модели редактирования сущности тега
        /// </summary>
        public static Tag Convert(this Tag tag, TagEditViewModel model)
        {
            tag.Name = model.Name;

            return tag;
        } 
    }
}
