using MyBlog.Data.Entities.Comments;

namespace MyBlog.Data.Repositories
{
    public class CommentRepository : Repository<CommentEntity>
    {
        public CommentRepository(MyBlogContext context) : base(context) { }
    }
}
