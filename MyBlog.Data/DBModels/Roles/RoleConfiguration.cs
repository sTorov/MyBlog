using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Data.DBModels.Roles
{
    /// <summary>
    /// Конфигурация для таблицы ролей
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles").HasKey(c => c.Id);
            builder.HasData(
                    new Role() { 
                        Id = 1, 
                        Name = "User", 
                        NormalizedName = "USER",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Description = "Стандартная роль в приложении",
                    },
                    new Role() { 
                        Id = 2, 
                        Name = "Moderator", 
                        NormalizedName = "MODERATOR",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Description = "Данная роль позволяет выполнять редактирование, удаление комментариев и статей в приложении"
                    },
                    new Role() { 
                        Id = 3, 
                        Name = "Admin" , 
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Description = "Роль с максимальными возможностями в приложении"
                    }
                );
        }
    }
}
