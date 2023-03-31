using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(MyBlogContext context) : base(context) { }

        public async override Task<List<Post>> GetAllAsync() => 
            await Set.Include(p => p.Tags).Include(p => p.Comments).ToListAsync();

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId) 
        {
            return await Task.Run(() => 
                Set.Include(p => p.Tags).Include(p => p.Comments)
                    .AsEnumerable().Where(p => p.UserId == userId).ToList());
        }
    }
}
