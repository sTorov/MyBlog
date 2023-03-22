namespace MyBlog.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(bool ensureAutoHistory);
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository) where TEntity : class;
    }
}
