namespace LiqPayProviderService.Domain.SeedWork;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
}