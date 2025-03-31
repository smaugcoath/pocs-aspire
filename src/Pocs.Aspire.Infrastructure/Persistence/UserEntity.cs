namespace Pocs.Aspire.Infrastructure.Persistence;

internal class UserEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
