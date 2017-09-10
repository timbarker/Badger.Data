using System.Data.Common;

namespace Badger.Data.Queries
{
    sealed class DbQueryScalarBuilder<T> 
        : DbBaseBuilder<IDbQueryScalarBuilder<T>>
        , IDbQueryScalarBuilder<T>
    {
        private T @default;

        public DbQueryScalarBuilder(DbCommand command)
            : base (command)
        {
        }

        public override IDbExecutor Build()
        {
            return new DbQueryScalarExecuter<T>(command, @default);
        }

        public IDbQueryScalarBuilder<T> WithDefault(T @default)
        {
            this.@default = @default;
            return this;
        }
    }
}
