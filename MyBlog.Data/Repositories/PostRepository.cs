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
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.User).Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId) =>
            await Set.Include(p => p.Users).Include(p => p.Tags).Include(p => p.Comments)
                .Where(p => p.UserId == userId).ToListAsync();

        public async Task<List<Post>> GetPostsByTagIdAsync(int tagId) =>
            await Set.Include(p => p.Tags).Include(p => p.Users).Include(p => p.Comments)
                .SelectMany(p => p.Tags, (p, t) => new { Post = p, TagId = t.Id })
                .Where(o => o.TagId == tagId).Select(o => o.Post).ToListAsync();

        public async Task<int> FindLastCreateIdByUserId(int userId) => 
            await Set.Where(p => p.UserId == userId).Select(p => p.Id)
            .OrderByDescending(id => id).FirstOrDefaultAsync();
    }
}
