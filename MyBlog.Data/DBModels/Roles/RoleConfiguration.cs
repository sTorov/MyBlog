using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Data.DBModels.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles").HasKey(c => c.Id);
            builder.HasData(
                    new Role("User") { Id = 1 },
                    new Role("Moderator") { Id = 2 },
                    new Role("Admin") { Id = 3 }
                );
        }
    }
}
