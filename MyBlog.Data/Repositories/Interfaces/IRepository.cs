namespace MyBlog.Data.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс базового репозитория для кастомных репозиториев
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получение всех сущностей указанного типа <typeparamref name="T"/>
        /// </summary>
        Task<List<T>> GetAllAsync();
        /// <summary>
        /// Получение сущности <typeparamref name="T"/> по её идентификатору
        /// </summary>
        Task<T?> GetAsync(int id);
        /// <summary>
        /// Создание сущности <typeparamref name="T"/>
        /// </summary>
        Task<int> CreateAsync(T item);
        /// <summary>
        /// Обновление сущности <typeparamref name="T"/>
        /// </summary>
        Task<int> UpdateAsync(T item);
        /// <summary>
        /// Удаление сущности <typeparamref name="T"/>
        /// </summary>
        Task<int> DeleteAsync(T item);
    }
}
