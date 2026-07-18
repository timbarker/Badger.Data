using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Commands;

internal sealed class PreparedCommand(DbCommand command) : IPreparedCommand
{
    public int Execute()
    {
        return command.ExecuteNonQuery();
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
