namespace Pocs.Aspire.Business.Users;

using OneOf;
using OneOf.Types;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

public interface IUserService
{
    Task<OneOf<User, ValidationError>> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<OneOf<User, NotFound>> GetAsync(UserId id, CancellationToken cancellationToken = default);
    Task<OneOf<User, NotFound, ValidationError>> UpdateAsync(User user, CancellationToken cancellationToken = default);

}