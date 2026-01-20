namespace LiqPayProviderService.Domain.SeedWork;

public interface IRepository<T> where T : class
{
    T Add(T entity, CancellationToken cancellationToken = default);
    Task Update(T entity);
    void Remove(T entity);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
}