using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Comments;

namespace MyBlog.Data.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(MyBlogContext context) : base(context) { }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId) =>
            await Set.Include(c => c.User).Where(c => c.PostId == postId).ToListAsync();

        public async Task<List<Comment>> GetCommentsByUserIdAsync(int userId) =>
            await Set.Where(c => c.UserId == userId).ToListAsync();
    }
}
