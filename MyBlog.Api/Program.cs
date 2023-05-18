using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlog.App.Utils.Extensions;
using MyBlog.Data;
using MyBlog.Data.DBModels.Comments;
using MyBlog.Data.DBModels.Posts;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Tags;
using MyBlog.Data.DBModels.Users;
using MyBlog.Data.Repositories;
using MyBlog.Services;
using MyBlog.Services.ApiModels.Users.Response;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MyBlog.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xml = $"{Assembly.GetAssembly(typeof(UserApiModel)).GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
                opt.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xml));
                opt.SupportNonNullableReferenceTypes();
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MyBlogContext>(opt => opt.UseSqlite(connectionString, b => b.MigrationsAssembly("MyBlog.Data")))
                .AddUnitOfWork()
                .AddCustomRepository<Post, PostRepository>()
                .AddCustomRepository<Comment, CommentRepository>()
                .AddCustomRepository<Tag, TagRepository>()
                .AddAppServices();

            var assembly = Assembly.GetAssembly(typeof(MapperProfile));
            builder.Services.AddAutoMapper(assembly);

            builder.Services.AddIdentity<User, Role>(cfg => {
                cfg.Password.RequiredLength = 8;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<MyBlogContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(o =>
                {
                    o.InjectStylesheet("/css/swagger-custom.css");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
