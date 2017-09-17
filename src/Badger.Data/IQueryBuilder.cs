using System;
using System.Collections.Generic;

namespace Badger.Data
{
    public interface IQueryBuilder
    {
        IQueryBuilder WithSql(string sql);
        IQueryBuilder WithParameter<TParam>(string name, TParam value);
        
        IQueryBuilder<IEnumerable<T>> WithMapper<T>(Func<IRow, T> mapper);
        IQueryBuilder<T> WithMapper<T>(Func<IRow, T> mapper, T @default);
        IQueryBuilder<T> WithDefault<T>(T @default);

    }

    public interface IQueryBuilder<T>
    {
        IPreparedQuery<T> Build();
    }
}
