namespace Pocs.Aspire.Business.Users;

using LanguageExt.Common;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

public interface IUserService
{
    Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<Result<User>> GetAsync(UserId id, CancellationToken cancellationToken = default);
    Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default);

}