using MyBlog.App.ViewModels.Tags.Response;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.App.Utils.Extensions
{
    public static class TagExtensions
    {
        public static Tag Convert(this Tag tag, TagEditViewModel model)
        {
            tag.Name = model.Name;

            return tag;
        } 
    }
}
