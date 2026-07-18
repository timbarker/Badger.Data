using System.Data.Common;

namespace Badger.Data.Queries;

internal sealed class QueryScalarBuilder<T>(DbCommand command, T @default) : IQueryBuilder<T>
{
    public IPreparedQuery<T> Build()
    {
        return new PreparedScalarQuery<T>(command, @default);
    }
}
