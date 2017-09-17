using System.Data.Common;

namespace Badger.Data.Queries
{
    internal sealed class QueryScalarBuilder<T> : IQueryBuilder<T>
    {
        private readonly DbCommand command;
        private readonly T @default;

        public QueryScalarBuilder(DbCommand command, T @default)
        {
            this.command = command;
            this.@default = @default;
        }
        public IPreparedQuery<T> Build()
        {
            return new PreparedScalarQuery<T>(this.command, this.@default);
        }
    }

}
