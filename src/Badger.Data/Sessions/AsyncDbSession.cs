using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Badger.Data.Queries;
using Badger.Data.Commands;

namespace Badger.Data.Sessions
{
    class AsyncDbSession : IAsyncDbSession
    {
        protected readonly DbConnection conn;

        public AsyncDbSession(DbConnection conn)
        {
            this.conn = conn;
        }

        public void Dispose()
        {
            conn.Dispose();
        }

        public async Task<int> ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            var builder = new Commands.DbCommandBuilder(await CreateCommandAsync(cancellationToken));
            return await ((IDbExecutor<int>)command.Build(builder)).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<int> ExecuteCommandAsync(ICommand command)
        {
            return ExecuteCommandAsync(command, CancellationToken.None);
        }

        public async Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var builder = new DbQueryBuilder<TResult>(await CreateCommandAsync(cancellationToken));
            return await ((IDbExecutor<IEnumerable<TResult>>)query.Build(builder)).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            return ExecuteQueryAsync(query, CancellationToken.None);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IScalarQuery<TResult> query, CancellationToken cancellationToken)
        {
            var builder = new DbScalarQueryBuilder<TResult>(await CreateCommandAsync(cancellationToken));
            return await ((IDbExecutor<TResult>)query.Build(builder)).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task<TResult> ExecuteQueryAsync<TResult>(IScalarQuery<TResult> query)
        {
            return ExecuteQueryAsync(query, CancellationToken.None);
        }

        protected virtual async Task<DbCommand> CreateCommandAsync(CancellationToken cancellationToken)
        {
            await EnsureConnectionAsync(cancellationToken).ConfigureAwait(false);

            return this.conn.CreateCommand();
        }

        private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
        {
            if (this.conn.State == ConnectionState.Closed)
                await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                
        }

        protected virtual async Task OpenConnectionAsync(CancellationToken cancellationToken)
        {
            await this.conn.OpenAsync(cancellationToken).ConfigureAwait(false); 
        }
    }
}
