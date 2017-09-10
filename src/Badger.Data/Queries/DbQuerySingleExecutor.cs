using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries
{
    sealed class DbQuerySingleExecutor<TResult> : IDbExecutor<TResult>
    {
        private readonly DbCommand command;
        private readonly Func<IDbRow, TResult> mapper;
        private readonly TResult @default;

        public DbQuerySingleExecutor(DbCommand command, Func<IDbRow, TResult> mapper, TResult @default)
        {
            this.command = command;
            this.mapper = mapper;
            this.@default = @default;
        }

        public TResult Execute()
        {
            using (var reader = this.command.ExecuteReader())
            {
                var row = new DbRow(reader);
                if (reader.Read())
                    return mapper.Invoke(row);
                
                return this.@default;
            }
        }

        public async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var reader = await this.command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                var row = new DbRow(reader);
                if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    return mapper.Invoke(row);

                return this.@default;
            }
        }
    }
}
