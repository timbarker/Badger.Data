using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal sealed class AsyncTransactionSession : AsyncSession, IAsyncTransactionSession
    {
        private readonly IsolationLevel isolationLevel;

        private DbTransaction transaction;

        public AsyncTransactionSession(DbConnection conn, IsolationLevel isolationLevel)
            : base (conn)
        {
            this.isolationLevel = isolationLevel;
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        protected override async Task OpenConnectionAsync(CancellationToken cancellationToken)
        {
            await base.OpenConnectionAsync(cancellationToken);
            this.transaction = this.conn.BeginTransaction(this.isolationLevel);
        }

        protected override async Task<DbCommand> CreateCommandAsync(CancellationToken cancellationToken)
        {
            var command = await base.CreateCommandAsync(cancellationToken);
            command.Transaction = this.transaction;
            return command;
        }
    }
}
