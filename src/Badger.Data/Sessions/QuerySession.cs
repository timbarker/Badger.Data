using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Badger.Data.Queries;

namespace Badger.Data.Sessions
{
    internal sealed class QuerySession : Session, IQuerySession
    {
        private readonly ParameterFactory _parameterFactory;

        public QuerySession(DbConnection connection, ParameterFactory parameterFactory, IsolationLevel isolationLevel)
            : base(connection, isolationLevel)
        {
            _parameterFactory = parameterFactory;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            var builder = new QueryBuilder(CreateCommand(), _parameterFactory);
            return query.Prepare(builder).Execute();
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var builder = new QueryBuilder(await CreateCommandAsync(cancellationToken), _parameterFactory);
            return await query.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
