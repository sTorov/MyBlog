using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;

namespace MyBlog.Data.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(MyBlogContext context) : base(context) { }

        public async override Task<List<Tag>> GetAllAsync() =>
            await Set.Include(t => t.Posts).ToListAsync();

        public async Task<Tag?> GetTagByNameAsync(string name) => 
            await Set.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Name == name);
    }
}
