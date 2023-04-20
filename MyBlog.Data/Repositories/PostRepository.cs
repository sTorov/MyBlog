using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(MyBlogContext context) : base(context) { }

        public async override Task<List<Post>> GetAllAsync() => 
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.Users).ToListAsync();

        public async override Task<Post?> GetAsync(int id) => 
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId) =>
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.Users)
                .Where(p => p.UserId == userId).ToListAsync();
    }
}
