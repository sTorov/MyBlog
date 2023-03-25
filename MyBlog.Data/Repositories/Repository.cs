using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Repositories.Interfaces;

namespace MyBlog.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _dbContext;
        public DbSet<T> Set { get; private set; }

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            var set = _dbContext.Set<T>();
            set.Load();

            Set = set;
        }

        public virtual async Task CreateAsync(T item)
        {
            await Set.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T item)
        {
            await Task.Run(() => Set.Remove(item));
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync() => await Set.ToListAsync();

        public virtual async Task<T?> GetAsync(int id) => await Set.FindAsync(id);

        public virtual async Task UpdateAsync(T item)
        {
            await Task.Run(() => Set.Update(item));
            await _dbContext.SaveChangesAsync();
        }
    }
}
