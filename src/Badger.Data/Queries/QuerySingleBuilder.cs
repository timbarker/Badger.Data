using System;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QuerySingleBuilder<T> : IQueryBuilder<T>
    {
        private readonly DbCommand command;
        private readonly Func<IRow, T> mapper;
        private readonly T @default;

        public QuerySingleBuilder(DbCommand command, Func<IRow, T> mapper, T @default)
        {
            this.command = command;
            this.mapper = mapper;
            this.@default = @default;
        }
        public IPreparedQuery<T> Build()
        {
            return new PreparedSingleQuery<T>(this.command, this.mapper, this.@default);
        }
    }

}
