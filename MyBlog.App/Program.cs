using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.App.Utils.Extensions;
using MyBlog.Data;
using MyBlog.Data.Entities.Comments;
using MyBlog.Data.Entities.Posts;
using MyBlog.Data.Entities.Tags;
using MyBlog.Data.Entities.Users;
using MyBlog.Data.Repositories;
using MyBlog.App.Utils;
using System.Reflection;

namespace MyBlog.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MyBlogContext>(opt => opt.UseSqlite(connectionString, b => b.MigrationsAssembly("MyBlog.Data")))
                .AddUnitOfWork()
                .AddCustomRepository<PostEntity, PostRepository>()
                .AddCustomRepository<CommentEntity, CommentRepository>()
                .AddCustomRepository<TagEntity, TagRepository>();

            var assembly = Assembly.GetAssembly(typeof(MapperProfile));
            builder.Services.AddAutoMapper(assembly);

            builder.Services.AddIdentity<UserEntity, IdentityRole<int>>()
                .AddEntityFrameworkStores<MyBlogContext>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
