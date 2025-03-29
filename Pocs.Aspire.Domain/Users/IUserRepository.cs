using Pocs.Aspire.Domain.Users.ValueObjects;

namespace Pocs.Aspire.Domain.Users;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id);
    Task UpdateAsync(User user);
    Task CreateAsync(User user);
    Task<bool> UserExistsAsync(UserId id);
}
