using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Data.Repositories
{
    /// <summary>
    /// Репозиторий тегов
    /// </summary>
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(MyBlogContext context) : base(context) { }

        public async override Task<List<Tag>> GetAllAsync() =>
            await Set.Include(t => t.Posts).ToListAsync();

        public override async Task<Tag?> GetAsync(int id) => 
            await Set.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Id == id);

        /// <summary>
        /// Получение тега по его названию
        /// </summary>
        public async Task<Tag?> GetTagByNameAsync(string name) => 
            await Set.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Name == name);

        /// <summary>
        /// Получение списка тегов для указанной статьи
        /// </summary>
        public async Task<List<Tag>> GetTagsByPostIdAsync(int postId) =>
            await Set.Include(t => t.Posts)
            .SelectMany(t => t.Posts, (t, p) => new { Tag = t, PostId = p.Id })
            .Where(o => o.PostId == postId).Select(o => o.Tag).ToListAsync();
    }
}
