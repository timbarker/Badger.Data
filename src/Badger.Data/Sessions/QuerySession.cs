using System.Data;
using System.Data.Common;
using Badger.Data.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal sealed class QuerySession : Session, IQuerySession
    {
        public QuerySession(DbConnection connection, IsolationLevel isolationLevel)
            : base(connection, isolationLevel)
        {
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            var builder = new QueryBuilder(CreateCommand());
            return query.Prepare(builder).Execute();
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var builder = new QueryBuilder(await CreateCommandAsync(cancellationToken));
            return await query.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
