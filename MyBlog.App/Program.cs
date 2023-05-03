using Microsoft.EntityFrameworkCore;
using MyBlog.App.Utils.Extensions;
using MyBlog.Data;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;
using System.Reflection;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Services;
using Microsoft.AspNetCore.Http.Extensions;

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
                .AddCustomRepository<Post, PostRepository>()
                .AddCustomRepository<Comment, CommentRepository>()
                .AddCustomRepository<Tag, TagRepository>()
                .AddAppServices()
                .AddControllerModules();

            var assembly = Assembly.GetAssembly(typeof(MapperProfile));
            builder.Services.AddAutoMapper(assembly);

            Console.WriteLine(builder.Environment.IsProduction());

            builder.Services.AddIdentity<User, Role>(cfg => {
                cfg.Password.RequiredLength = 8;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<MyBlogContext>();

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Login";
                opt.AccessDeniedPath = "/AccessDenied";
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePages(async statusCodeContext =>
            {
                var response = statusCodeContext.HttpContext.Response;

                response.ContentType = "text/plain; charset=UTF-8";
                if (response.StatusCode == 400)
                    response.Redirect("/BadRequest");

                else if (response.StatusCode == 404)
                    response.Redirect("/NotFound");

                else if (response.StatusCode == 401)
                {
                    var returnUrl = statusCodeContext.HttpContext.Request.GetEncodedPathAndQuery().Replace("/", "%2F");
                    response.Redirect($"/Login?ReturnUrl={returnUrl}");
                }
            });

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
