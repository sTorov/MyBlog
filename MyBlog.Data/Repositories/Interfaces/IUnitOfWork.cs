namespace MyBlog.Data.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации методов для получения кастомного репозитория и сохранения изменений в БД
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Сохранение изменений в БД
        /// </summary>
        int SaveChanges(bool ensureAutoHistory = false);
        /// <summary>
        /// Получение кастомного репозитория типа <typeparamref name="TEntity"/>
        /// </summary>
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
