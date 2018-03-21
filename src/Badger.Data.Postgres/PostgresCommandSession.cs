using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Badger.Data.Commands;
using Badger.Data.Sessions;

namespace Badger.Data.Postgres
{
    internal sealed class PostgresCommandSession : CommandSession, IPostgresCommandSession
    {
        private readonly ParameterFactory _parameterFactory;

        public PostgresCommandSession(DbConnection connection, ParameterFactory parameterFactory,
            IsolationLevel isolationLevel) : base(connection, parameterFactory, isolationLevel)
        {
            _parameterFactory = parameterFactory;
        }

        public InsertResult<T> ExecuteInsert<T>(ICommand insertCommand, string identifierName)
        {
            var command = CreateCommand();
            var builder = new CommandBuilder(command, _parameterFactory);
            builder.AsInsert(identifierName);

            var rowsAffected = insertCommand.Prepare(builder).Execute();
            return InsertResult<T>.CreateResult(rowsAffected, command.Parameters[identifierName].Value);
        }

        public async Task<InsertResult<T>> ExecuteInsertAsync<T>(ICommand insertCommand, string identifierName,
            CancellationToken cancellationToken = default)
        {
            var command = CreateCommand();
            var builder = new CommandBuilder(command, _parameterFactory);
            builder.AsInsert(identifierName);

            var rowsAffected =
                await insertCommand.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            return InsertResult<T>.CreateResult(rowsAffected, command.Parameters[identifierName].Value);
        }
    }
}
