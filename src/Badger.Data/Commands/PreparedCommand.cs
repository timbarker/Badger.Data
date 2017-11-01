using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Commands
{
    internal sealed class PreparedCommand : IPreparedCommand
    {
        private readonly DbCommand _command;

        public PreparedCommand(DbCommand command)
        {
            this._command = command;
        }

        public int Execute()
        {
            return _command.ExecuteNonQuery();
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
