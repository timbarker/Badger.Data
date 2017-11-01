using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries
{
    internal sealed class PreparedScalarQuery<T> : IPreparedQuery<T>
    {
        private readonly DbCommand _command;
        private readonly T _default;

        public PreparedScalarQuery(DbCommand command, T @default)
        {
            this._command = command;
            this._default = @default;
        }
        public T Execute()
        {
            var result = _command.ExecuteScalar();
            if (result == DBNull.Value) return _default;
            return (T)result;
        }

        public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = await _command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            if (result == DBNull.Value) return _default;
            return (T)result;
        }
    }
}
