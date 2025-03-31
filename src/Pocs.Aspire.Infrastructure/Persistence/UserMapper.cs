using Pocs.Aspire.Domain.Users;

namespace Pocs.Aspire.Infrastructure.Persistence;

internal static class UserMapper
{
    internal static User ToDomain(this UserEntity entity) =>
        new()
        {
            Id = entity.UserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email
        };

    internal static UserEntity ToEntity(this User user) =>
        new()
        {
            UserId = user.Id.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
}
