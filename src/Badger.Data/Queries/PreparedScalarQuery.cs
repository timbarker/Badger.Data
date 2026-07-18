using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries;

internal sealed class PreparedScalarQuery<T>(DbCommand command, T @default) : IPreparedQuery<T>
{
    public T Execute()
    {
        var result = command.ExecuteScalar();
        if (result == DBNull.Value) return @default;
        return (T)result;
    }

    public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        if (result == DBNull.Value) return @default;
        return (T)result;
    }
}
