using MyBlog.Data.Entities.Posts;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : Repository<PostEntity>
    {
        public PostRepository(MyBlogContext context) : base(context) { }
    }
}
