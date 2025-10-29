//https://github.com/dotnet-architecture/eShopOnContainersAI/blob/dev/src/Services/Ordering/Ordering.Domain/SeedWork/IUnitOfWork.cs

namespace LiqPayProviderService.Domain;


public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}