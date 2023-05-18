using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Services.ViewModels.Posts.Response
{
    /// <summary>
    /// Модель представления всез статей
    /// </summary>
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
