using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries;

internal sealed class PreparedManyQuery<TResult>(DbCommand command, Func<IRow, TResult> mapper) : IPreparedQuery<IEnumerable<TResult>>
{
    public IEnumerable<TResult> Execute()
    {
        using var reader = command.ExecuteReader();
        var row = new Row(reader);
        while (reader.Read())
        {
            yield return mapper.Invoke(row);
        }
    }

    public async Task<IEnumerable<TResult>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = new List<TResult>();

        using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
        {
            var row = new Row(reader);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                result.Add(mapper.Invoke(row));
            }
        }

        return result;
    }
}
