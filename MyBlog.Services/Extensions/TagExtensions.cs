using MyBlog.Services.ViewModels.Tags.Response;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Services.Extensions
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
