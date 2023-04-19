namespace MyBlog.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<int> CreateAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<int> DeleteAsync(T item);
    }
}
