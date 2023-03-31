using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Data.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(MyBlogContext context) : base(context) { }

        public async Task<List<Comment>> GetCommentsByPostId(int postId) =>
            await Task.Run(() => Set.AsEnumerable().Where(c => c.PostId == postId).ToList());
    }
}
