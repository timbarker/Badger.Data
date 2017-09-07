using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IDbScalarQuery<TResult>
    {
        Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
        TResult Execute();
    }
}
