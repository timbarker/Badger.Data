using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryBuilder : Builder<IQueryBuilder>, IQueryBuilder
    {
        public QueryBuilder(DbCommand command)
            : base(command)
        {
        }

        public IQueryBuilder<T> WithScalar<T>(T @default = default(T))
        {
            return new QueryScalarBuilder<T>(this.command, @default);
        }

        public IQueryBuilder<IEnumerable<T>> WithMapper<T>(Func<IRow, T> mapper)
        {
            return new QueryManyBuilder<T>(this.command, mapper);
        }

        public IQueryBuilder<T> WithSingleMapper<T>(Func<IRow, T> mapper, T @default = default(T))
        {
            return new QuerySingleBuilder<T>(this.command, mapper, @default);
        }
    }
}
