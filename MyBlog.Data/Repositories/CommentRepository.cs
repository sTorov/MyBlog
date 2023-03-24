using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Data.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(MyBlogContext context) : base(context) { }
    }
}
