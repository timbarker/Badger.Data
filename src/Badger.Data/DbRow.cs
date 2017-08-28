using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data
{
    class DbRow : IDbRow
    {
        private readonly DbDataReader reader;

        private static readonly IDictionary<Type, Func<DbDataReader, int, object>> Readers = 
            new Dictionary<Type, Func<DbDataReader, int, object>> 
            {
                [typeof(int)] = (r, i) => r.GetInt32(i),
                [typeof(long)] = (r, i) => r.GetInt64(i),
                [typeof(string)] = (r, i) => r.GetString(i),
                [typeof(DateTime)] = (r, i) => r.GetDateTime(i)
            };

        public DbRow(DbDataReader reader)
        {
            this.reader = reader;
        }
        public T Get<T>(string column)
        {
            return (T)Readers[typeof(T)].Invoke(this.reader, this.reader.GetOrdinal(column));
        }
    }
}
