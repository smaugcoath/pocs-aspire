using LanguageExt;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.Domain.Users;
public interface IUserRepository
{
    Task<Option<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<Unit> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<Unit> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsExceptForUser(Email email, UserId? userId = null, CancellationToken cancellationToken= default);
}
