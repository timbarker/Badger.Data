using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryManyBuilder<T> : IQueryBuilder<IEnumerable<T>>
    {
        private readonly DbCommand command;
        private readonly Func<IRow, T> mapper;

        public QueryManyBuilder(DbCommand command, Func<IRow, T> mapper)
        {
            this.command = command;
            this.mapper = mapper;
        }

        public IPreparedQuery<IEnumerable<T>> Build()
        {
            return new PreparedManyQuery<T>(this.command, this.mapper);
        }
    }

}
