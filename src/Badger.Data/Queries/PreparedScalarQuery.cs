using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries
{
    internal sealed class PreparedScalarQuery<T> : IPreparedQuery<T>
    {
        private readonly DbCommand command;
        private readonly T @default;

        public PreparedScalarQuery(DbCommand command, T @default)
        {
            this.command = command;
            this.@default = @default;
        }
        public T Execute()
        {
            var result = this.command.ExecuteScalar();
            if (result == DBNull.Value) return @default;
            return (T)result;
        }

        public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = await this.command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            if (result == DBNull.Value) return @default;
            return (T)result;
        }
    }
}
