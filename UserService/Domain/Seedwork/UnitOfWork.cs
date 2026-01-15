using UserService.Infrastructure.Context;

namespace UserService.Domain.SeedWork;



public class UnitOfWork(UsersDbContext context) : IUnitOfWork
{
    private bool _disposed;
    private readonly UsersDbContext _context = context;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _context?.Dispose();
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~UnitOfWork()
    {
        Dispose(false);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}