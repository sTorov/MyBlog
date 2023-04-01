using MyBlog.Data.DBModels.Tags;

namespace MyBlog.App.ViewModels.Tags
{
    public class TagsViewModel
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
