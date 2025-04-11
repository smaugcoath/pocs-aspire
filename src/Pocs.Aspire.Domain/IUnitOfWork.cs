using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.Domain;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
