using System.Data.Common;

namespace Badger.Data
{
    class DbScalarQueryBuilder<T> : DbBaseBuilder<IDbScalarQueryBuilder<T>>, IDbScalarQueryBuilder<T>
    {
        private T @default;

        public DbScalarQueryBuilder(DbCommand command)
            : base (command)
        {
        }

        public IDbScalarQuery<T> Build()
        {
            return new DbQueryScalarExecuter<T>(command, @default);
        }

        public IDbScalarQueryBuilder<T> WithDefault(T @default)
        {
            this.@default = @default;
            return this;
        }
    }
}
