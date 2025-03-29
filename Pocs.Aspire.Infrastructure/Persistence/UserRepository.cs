using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;


namespace Pocs.Aspire.Infrastructure.Persistence;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        var query = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == id.Value);

        var result = query?.ToDomain();
        return result;
    }

    public async Task CreateAsync(User user)
    {
        var entity = user.ToEntity();
        await _context.Users.AddAsync(entity);
    }

    public async Task UpdateAsync(User user)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(e => e.UserId == user.Id.Value);

        if (entity is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.Email = user.Email;

        await Task.CompletedTask;
    }

    public async Task<bool> UserExistsAsync(UserId id)
    {
        var exists = await _context.Users.AnyAsync(x => x.UserId == id.Value);

        return exists;
    }
}
