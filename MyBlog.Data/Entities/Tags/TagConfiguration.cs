using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Data.Entities.Tags
{
    public class TagConfiguration : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable("Tags").HasKey(c => c.Id);
        }
    }
}
