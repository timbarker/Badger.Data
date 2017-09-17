using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Commands
{
    internal sealed class PreparedCommand : IPreparedCommand
    {
        private readonly DbCommand command;

        public PreparedCommand(DbCommand command)
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
