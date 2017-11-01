using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryScalarBuilder<T> : IQueryBuilder<T>
    {
        private readonly DbCommand _command;
        private readonly T _default;

        public QueryScalarBuilder(DbCommand command, T @default)
        {
            this._command = command;
            this._default = @default;
        }
        public IPreparedQuery<T> Build()
        {
            return new PreparedScalarQuery<T>(_command, _default);
        }
    }

}
