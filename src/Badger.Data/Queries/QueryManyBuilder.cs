using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryManyBuilder<T> : IQueryBuilder<IEnumerable<T>>
    {
        private readonly DbCommand _command;
        private readonly Func<IRow, T> _mapper;

        public QueryManyBuilder(DbCommand command, Func<IRow, T> mapper)
        {
            this._command = command;
            this._mapper = mapper;
        }

        public IPreparedQuery<IEnumerable<T>> Build()
        {
            return new PreparedManyQuery<T>(_command, _mapper);
        }
    }

}
