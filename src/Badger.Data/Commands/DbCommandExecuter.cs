using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Commands
{
    sealed class DbCommandExecuter : IDbCommand
    {
        private readonly DbCommand command;

        public DbCommandExecuter(DbCommand command)
        {
            this.command = command;
        }

        public int Execute()
        {
            return this.command.ExecuteNonQuery();
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await this.command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
