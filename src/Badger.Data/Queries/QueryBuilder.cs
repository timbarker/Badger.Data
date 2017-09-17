using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryBuilder : BaseBuilder<IQueryBuilder>, IQueryBuilder
    {
        public QueryBuilder(DbCommand command)
            : base(command)
        {
        }

        public IQueryBuilder<T> WithDefault<T>(T @default)
        {
            return new QueryScalarBuilder<T>(this.command, @default);
        }

        public IQueryBuilder<IEnumerable<T>> WithMapper<T>(Func<IRow, T> mapper)
        {
            return new QueryManyBuilder<T>(this.command, mapper);
        }

        public IQueryBuilder<T> WithMapper<T>(Func<IRow, T> mapper, T @default)
        {
            return new QuerySingleBuilder<T>(this.command, mapper, @default);
        }
    }
}
