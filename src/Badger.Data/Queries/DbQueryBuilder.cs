using System;
using System.Data.Common;

namespace Badger.Data.Queries
{
    sealed class DbQueryBuilder<T> : DbBaseBuilder<IDbQueryBuilder<T>>, IDbQueryBuilder<T>
    {
        private Func<IDbRow, T> mapper;

        public DbQueryBuilder(DbCommand command)
            : base(command)
        {
        }

        public IDbQueryBuilder<T> WithMapper(Func<IDbRow, T> mapper)
        {
            this.mapper = mapper;
            return this;
        }

        public override IDbExecutor Build()
        {
            return new DbQueryExecuter<T>(command, mapper);
        }
    }
}
