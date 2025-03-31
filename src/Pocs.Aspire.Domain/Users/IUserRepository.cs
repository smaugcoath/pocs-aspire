using Pocs.Aspire.Domain.Users.ValueObjects;

namespace Pocs.Aspire.Domain.Users;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(UserId id, CancellationToken cancellationToken = default);
}
