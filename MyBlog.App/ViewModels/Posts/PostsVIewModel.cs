using MyBlog.Data.DBModels.Posts;

namespace MyBlog.App.ViewModels.Posts
{
    public class PostsVIewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
