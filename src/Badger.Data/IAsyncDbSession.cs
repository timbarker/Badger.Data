using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IAsyncDbSession : IDisposable
    {
        Task<int> ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken);
        Task<int> ExecuteCommandAsync(ICommand command);

        Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query);

        Task<TResult> ExecuteQueryAsync<TResult>(IQueryScalar<TResult> query, CancellationToken cancellationToken);
        Task<TResult> ExecuteQueryAsync<TResult>(IQueryScalar<TResult> query);
    }
}
