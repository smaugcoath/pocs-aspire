using LanguageExt;
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

    public async Task<Option<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        var result = await _context.Users.FindAsync(id);

        return result;
    }

    public async Task<Unit> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(entity);

        return Unit.Default;
    }

    public async Task<Unit> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);

        await Task.CompletedTask;

        return Unit.Default;
    }

    public async Task<bool> EmailExistsExceptForUser(Email email, UserId? userId = null, CancellationToken cancellationToken = default)
    {
        var result = await _context.Users.AsNoTracking().AnyAsync(x => x.Email == email && x.Id != userId);

        return result;
    }
}
