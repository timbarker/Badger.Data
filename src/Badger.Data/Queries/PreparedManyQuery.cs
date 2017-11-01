using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries
{
    internal sealed class PreparedManyQuery<TResult> : IPreparedQuery<IEnumerable<TResult>>
    {
        private readonly DbCommand _command;
        private readonly Func<IRow, TResult> _mapper;

        public PreparedManyQuery(DbCommand command, Func<IRow, TResult> mapper)
        {
            this._command = command;
            this._mapper = mapper;
        }

        public IEnumerable<TResult> Execute()
        {
            using (var reader = _command.ExecuteReader())
            {
                var row = new Row(reader);
                while (reader.Read())
                {
                    yield return _mapper.Invoke(row);
                }
            }
        }

        public async Task<IEnumerable<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = new List<TResult>();
            
            using (var reader = await _command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                var row = new Row(reader);
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    result.Add(_mapper.Invoke(row));
                }
            }

            return result;
        }
    }
}
