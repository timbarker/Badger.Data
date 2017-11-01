using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Queries
{
    internal sealed class PreparedSingleQuery<TResult> : IPreparedQuery<TResult>
    {
        private readonly DbCommand _command;
        private readonly Func<IRow, TResult> _mapper;
        private readonly TResult _default;

        public PreparedSingleQuery(DbCommand command, Func<IRow, TResult> mapper, TResult @default)
        {
            this._command = command;
            this._mapper = mapper;
            this._default = @default;
        }

        public TResult Execute()
        {
            using (var reader = _command.ExecuteReader())
            {
                var row = new Row(reader);
                if (reader.Read())
                    return _mapper.Invoke(row);
                
                return _default;
            }
        }

        public async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var reader = await _command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                var row = new Row(reader);
                if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    return _mapper.Invoke(row);

                return _default;
            }
        }
    }
}
