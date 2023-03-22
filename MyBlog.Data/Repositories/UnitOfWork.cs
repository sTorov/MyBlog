using Microsoft.EntityFrameworkCore.Infrastructure;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _context;
        private Dictionary<Type, object>? _repositories;
        private bool isDisposed = false;

        public UnitOfWork(MyBlogContext context) 
        {
            _context = context;
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true)
            where TEntity : class
        {
            if(_repositories == null)
                _repositories = new Dictionary<Type, object>();

            if (hasCustomRepository)
            {
                var customRepo = _context.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                    return customRepo;
            }

            var type = typeof(TEntity);
            if (_repositories.ContainsKey(type))
                _repositories[type] = new Repository<TEntity>(_context);

            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges(bool ensureAutoHistory = false) =>
            _context.SaveChanges(ensureAutoHistory);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if(isDisposed)
                return;

            if(disposing)
                _context.Dispose();

            isDisposed = true;
        }
    }
}
