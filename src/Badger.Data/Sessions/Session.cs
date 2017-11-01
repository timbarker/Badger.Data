using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal abstract class Session : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly IsolationLevel _isolationLevel;
        private DbTransaction _transaction;

        protected Session(DbConnection connection, IsolationLevel isolationLevel)
        {
            this._connection = connection;
            this._isolationLevel = isolationLevel;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        protected DbCommand CreateCommand()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction(_isolationLevel);
            }

            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }

        protected async Task<DbCommand> CreateCommandAsync(CancellationToken cancellationToken)
        {
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync(cancellationToken);
                _transaction = _connection.BeginTransaction(_isolationLevel);
            }

            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }
    }
}
