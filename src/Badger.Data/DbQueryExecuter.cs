using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    class DbQueryExecuter<TResult> : IDbQuery<TResult>
    {
        private readonly DbCommand command;
        private readonly Func<IDbRow, TResult> mapper;

        public DbQueryExecuter(DbCommand command, Func<IDbRow, TResult> mapper)
        {
            this.command = command;
            this.mapper = mapper;
        }

        public IEnumerable<TResult> Execute()
        {
            using (var reader = this.command.ExecuteReader())
            {
                var row = new DbRow(reader);
                while (reader.Read())
                {
                    yield return mapper.Invoke(row);
                }
            }
        }

        public async Task<IEnumerable<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = new List<TResult>();
            
            using (var reader = await this.command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                var row = new DbRow(reader);
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    result.Add(mapper.Invoke(row));
                }
            }

            return result;
        }
    }
}
