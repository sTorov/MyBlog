using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Services.ViewModels.Tags.Response
{
    /// <summary>
    /// Модель представления всех тегов
    /// </summary>
    public class TagsViewModel
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
