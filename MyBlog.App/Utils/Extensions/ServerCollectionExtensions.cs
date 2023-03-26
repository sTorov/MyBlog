using MyBlog.App.Utils.Services;
using MyBlog.App.Utils.Services.Interfaces;
using MyBlog.Data.Repositories;
using MyBlog.Data.Repositories.Interfaces;

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();

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
