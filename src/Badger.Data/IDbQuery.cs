using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IDbQuery<TResult>
    {
        Task<IEnumerable<TResult>> ExecuteAsync(CancellationToken cancellationToken);
        IEnumerable<TResult> Execute();
    }
}
