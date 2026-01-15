using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;


public class UserRepository(UsersDbContext context) : IUserRepository
{

    private readonly UsersDbContext _context = context;
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
    await _context.Users.ToListAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task InsertAsync(User entity, CancellationToken cancellationToken = default) =>
    await _context.Users.AddAsync(entity, cancellationToken);

    public void RemoveAsync(User entity) => _context.Users.Remove(entity);

    public Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }
}