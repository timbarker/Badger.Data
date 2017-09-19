using System;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IQuerySession : IDisposable
    {
        TResult Execute<TResult>(IQuery<TResult> query);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}
