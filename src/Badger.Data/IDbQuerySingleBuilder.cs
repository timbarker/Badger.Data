using System;

namespace Badger.Data
{
    public interface IDbQuerySingleBuilder<T> : IDbExecutorBuilder
    {
        IDbQuerySingleBuilder<T> WithSql(string sql);
        IDbQuerySingleBuilder<T> WithParameter<TParam>(string name, TParam value);
        IDbQuerySingleBuilder<T> WithMapper(Func<IDbRow, T> mapper);
        IDbQuerySingleBuilder<T> WithDefault(T @default);
    }
}
