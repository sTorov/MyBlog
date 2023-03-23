using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Entities.Comments;
using MyBlog.Data.Entities.Posts;
using MyBlog.Data.Entities.Tags;
using MyBlog.Data.Entities.Users;

namespace MyBlog.Data
{
    public class MyBlogContext : DbContext
    {
        public MyBlogContext(DbContextOptions<MyBlogContext> contextOptions) : base(contextOptions) =>
            Database.Migrate();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new СommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
