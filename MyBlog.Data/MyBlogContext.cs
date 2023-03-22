using Microsoft.EntityFrameworkCore;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;

namespace MyBlog.Data
{
    public class MyBlogContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Tag> Tags { get; set; }

        public MyBlogContext(DbContextOptions<MyBlogContext> contextOptions) : base(contextOptions)
        {
            Database.Migrate();
        }
    }
}
