using System;
using System.Collections.Generic;

namespace Badger.Data
{
    public interface IDbQueryBuilder<T>
    {
        IDbQueryBuilder<T> WithSql(string sql);
        IDbQueryBuilder<T> WithParameter<TParam>(string name, TParam value);
        IDbQueryBuilder<T> WithMapper(Func<IDbRow, T> mapper);
        IDbQuery<T> Build();
    }

}
