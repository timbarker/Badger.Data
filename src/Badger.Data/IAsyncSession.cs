using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IAsyncSession : IDisposable
    {
        Task<int> ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken);
        Task<int> ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
