using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBlog.Data.DBModels.Comments
{
    /// <summary>
    /// Конфигурации для таблицы комментариев
    /// </summary>
    public class СommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments").HasKey(c => c.Id);
        }
    }
}
