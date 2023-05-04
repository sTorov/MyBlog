using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;

namespace MyBlog.Data.Repositories
{
    /// <summary>
    /// Репозиторий статей
    /// </summary>
    public class PostRepository : Repository<Post>
    {
        public PostRepository(MyBlogContext context) : base(context) { }

        public async override Task<List<Post>> GetAllAsync() => 
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.Users).ToListAsync();

        public async override Task<Post?> GetAsync(int id) => 
            await Set.Include(p => p.Tags).Include(p => p.Comments).Include(p => p.User).Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Получение списка статей указанного пользователя
        /// </summary>
        public async Task<List<Post>> GetPostsByUserIdAsync(int userId) =>
            await Set.Include(p => p.Users).Include(p => p.Tags).Include(p => p.Comments)
                .Where(p => p.UserId == userId).ToListAsync();

        /// <summary>
        /// Получение списка статей, умеющих указанный тег
        /// </summary>
        public async Task<List<Post>> GetPostsByTagIdAsync(int tagId) =>
            await Set.Include(p => p.Tags).Include(p => p.Users).Include(p => p.Comments)
                .SelectMany(p => p.Tags, (p, t) => new { Post = p, TagId = t.Id })
                .Where(o => o.TagId == tagId).Select(o => o.Post).ToListAsync();
        
        /// <summary>
        /// Получение идентификатора последней созданной пользователем статьи 
        /// </summary>
        public async Task<int> FindLastCreateIdByUserId(int userId) => 
            await Set.Where(p => p.UserId == userId).Select(p => p.Id)
            .OrderByDescending(id => id).FirstOrDefaultAsync();
    }
}
