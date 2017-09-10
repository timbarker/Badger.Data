using System;
using System.Data.Common;

namespace Badger.Data.Queries
{
    sealed class DbQuerySingleBuilder<TResult> : DbBaseBuilder<IDbQuerySingleBuilder<TResult>>, IDbQuerySingleBuilder<TResult>
    {
        private TResult @default;
        private Func<IDbRow, TResult> mapper;


        public DbQuerySingleBuilder(DbCommand command)
            : base (command)
        { 
        }

        public override IDbExecutor Build()
        {
            return new DbQuerySingleExecutor<TResult>(this.command, this.mapper, this.@default);
        }

        public IDbQuerySingleBuilder<TResult> WithDefault(TResult @default)
        {
            this.@default = @default;
            return this;
        }

        public IDbQuerySingleBuilder<TResult> WithMapper(Func<IDbRow, TResult> mapper)
        {
            this.mapper = mapper;
            return this;
        }
    } 
}
