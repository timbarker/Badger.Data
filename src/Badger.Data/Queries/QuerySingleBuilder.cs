using System;
using System.Data.Common;

namespace Badger.Data.Queries;

internal sealed class QuerySingleBuilder<T>(DbCommand command, Func<IRow, T> mapper, T @default) : IQueryBuilder<T>
{
    public IPreparedQuery<T> Build()
    {
        return new PreparedSingleQuery<T>(command, mapper, @default);
    }
}
