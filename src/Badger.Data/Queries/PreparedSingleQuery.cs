using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries;

internal sealed class PreparedSingleQuery<TResult>(DbCommand command, Func<IRow, TResult> mapper, TResult @default) : IPreparedQuery<TResult>
{
    public TResult Execute()
    {
        using var reader = command.ExecuteReader();
        var row = new Row(reader);
        if (reader.Read())
            return mapper.Invoke(row);

        return @default;
    }

    public async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
    {
        using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
        {
            var row = new Row(reader);
            if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                return mapper.Invoke(row);

            return @default;
        }
    }
}
