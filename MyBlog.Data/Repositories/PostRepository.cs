using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(MyBlogContext context) : base(context) { }
    }
}
