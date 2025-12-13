

namespace UserService.Domain.SeedWork;

public interface IRepository<T> where T : class
{
    Task InsertAsync(T entity, CancellationToken cancellationToken = default);

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity);
    void RemoveAsync(T entity);

}