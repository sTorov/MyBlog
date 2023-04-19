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

        public virtual async Task<int> CreateAsync(T item)
        {
            await Set.AddAsync(item);
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(T item)
        {
            await Task.Run(() => Set.Remove(item));
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<T>> GetAllAsync() => await Set.ToListAsync();

        public virtual async Task<T?> GetAsync(int id) => await Set.FindAsync(id);

        public virtual async Task<int> UpdateAsync(T item)
        {
            await Task.Run(() => Set.Update(item));
            return await _dbContext.SaveChangesAsync();
        }
    }
}
