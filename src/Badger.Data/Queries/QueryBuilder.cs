using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryBuilder : Builder<IQueryBuilder>, IQueryBuilder
    {
        public QueryBuilder(DbCommand command, ParameterFactory parameterFactory)
            : base(command, parameterFactory)
        {
        }

        public IQueryBuilder<T> WithScalar<T>(T @default = default)
        {
            return new QueryScalarBuilder<T>(Command, @default);
        }

        public IQueryBuilder<IEnumerable<T>> WithMapper<T>(Func<IRow, T> mapper)
        {
            return new QueryManyBuilder<T>(Command, mapper);
        }

        public IQueryBuilder<T> WithSingleMapper<T>(Func<IRow, T> mapper, T @default = default)
        {
            return new QuerySingleBuilder<T>(Command, mapper, @default);
        }
    }
}
