using System;
using System.Collections.Generic;

namespace Badger.Data
{
    public interface IDbQueryBuilder
    {
        IDbQueryBuilder WithSql(string sql);
        IDbQueryBuilder WithParameter<T>(string name, T value);
        T ExecuteScalar<T>(T @default = default(T));      
        T ExecuteSingle<T>(Func<IDbRow, T> mapper);
        IEnumerable<T> Execute<T>(Func<IDbRow, T> mapper);
    }
}
