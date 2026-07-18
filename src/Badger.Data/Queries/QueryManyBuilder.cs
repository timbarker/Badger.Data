using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries;

internal sealed class QueryManyBuilder<T>(DbCommand command, Func<IRow, T> mapper) : IQueryBuilder<IEnumerable<T>>
{
    public IPreparedQuery<IEnumerable<T>> Build()
    {
        return new PreparedManyQuery<T>(command, mapper);
    }
}
