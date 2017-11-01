using System;
using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QuerySingleBuilder<T> : IQueryBuilder<T>
    {
        private readonly DbCommand _command;
        private readonly Func<IRow, T> _mapper;
        private readonly T _default;

        public QuerySingleBuilder(DbCommand command, Func<IRow, T> mapper, T @default)
        {
            this._command = command;
            this._mapper = mapper;
            this._default = @default;
        }
        public IPreparedQuery<T> Build()
        {
            return new PreparedSingleQuery<T>(_command, _mapper, _default);
        }
    }

}
