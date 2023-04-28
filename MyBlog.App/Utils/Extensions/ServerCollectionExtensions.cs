using MyBlog.App.Utils.Modules;
using MyBlog.App.Utils.Modules.Interfaces;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;
using MyBlog.Services.Services;
using MyBlog.Services.Services.Interfaces;

namespace MyBlog.App.Utils.Extensions
{
    public static class ServerCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }

        public static IServiceCollection AddControllerModules(this IServiceCollection services)
        {
            services.AddScoped<IUserControllerModule, UserControllerModule>();
            services.AddScoped<IRoleControllerModule, RoleControllerModule>();
            services.AddScoped<IPostControllerModule, PostControllerModule>();
            services.AddScoped<ITagControllerModule, TagControllerModule>();

            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, TRepoitory>(this IServiceCollection services) 
            where TEntity : class
            where TRepoitory : Repository<TEntity>
        {
            services.AddScoped<IRepository<TEntity>, TRepoitory>();

            return services;
        }
    }
}
