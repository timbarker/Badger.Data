using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Postgres
{
    public interface IPostgresCommandSession : ICommandSession
    {
        InsertResult<T> ExecuteInsert<T>(ICommand insertCommand, string identifierName);
        Task<InsertResult<T>> ExecuteInsertAsync<T>(ICommand command, string identifierName, CancellationToken cancellationToken = default);
    }
}
