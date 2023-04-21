using MyBlog.Data.DBModels.Posts;

namespace MyBlog.App.ViewModels.Posts.Request
{
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
