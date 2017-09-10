using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IDbExecutor {}

    interface IDbExecutor<TResult> : IDbExecutor 
    {
        Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
        TResult Execute();
    }
}
