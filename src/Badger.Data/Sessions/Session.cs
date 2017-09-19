using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal abstract class Session : IDisposable
    {
        private readonly DbConnection connection;
        private readonly IsolationLevel isolationLevel;
        private DbTransaction transaction;

        protected Session(DbConnection connection, IsolationLevel isolationLevel)
        {
            this.connection = connection;
            this.isolationLevel = isolationLevel;
        }

        public void Dispose()
        {
            this.transaction?.Dispose();
            this.connection.Dispose();
        }

        public void Commit()
        {
            this.transaction?.Commit();
        }

        protected DbCommand CreateCommand()
        {
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
                this.transaction = this.connection.BeginTransaction(this.isolationLevel);
            }

            var command = this.connection.CreateCommand();
            command.Transaction = this.transaction;
            return command;
        }

        protected async Task<DbCommand> CreateCommandAsync(CancellationToken cancellationToken)
        {
            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync(cancellationToken);
                this.transaction = this.connection.BeginTransaction(this.isolationLevel);
            }

            var command = this.connection.CreateCommand();
            command.Transaction = this.transaction;
            return command;
        }
    }
}
