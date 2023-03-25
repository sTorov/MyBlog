using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data.Repositories
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(MyBlogContext context) : base(context) { }

        public override Task<List<Post>> GetAllAsync()
        {
            Set.Include(p => p.Tags).Include(p => p.Comments);
            return base.GetAllAsync();
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId) 
        {
            Set.Include(p => p.Tags).Include(p => p.Comments);
            return await Task.Run(() => 
                Set.AsEnumerable().Where(p => p.UserId == userId).ToList());
        }
    }
}
