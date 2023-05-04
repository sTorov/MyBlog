using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Data.DBModels.Posts
{
    /// <summary>
    /// Конфигурация для таблицы статьи
    /// </summary>
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts").HasKey(c => c.Id);

            builder
                .HasMany(e => e.Users)
                .WithMany(e => e.VisitedPosts)
                .UsingEntity("UserToVisitedPost");
        }
    }
}
