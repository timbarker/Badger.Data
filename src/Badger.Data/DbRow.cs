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
                [typeof(char)] = (r, i) => r.GetChar(i),
                [typeof(bool)] = (r, i) => r.GetBoolean(i),
                [typeof(short)] = (r, i) => r.GetInt16(i),
                [typeof(int)] = (r, i) => r.GetInt32(i),
                [typeof(long)] = (r, i) => r.GetInt64(i),
                [typeof(float)] = (r, i) => r.GetFloat(i),
                [typeof(double)] = (r, i) => r.GetDouble(i),
                [typeof(decimal)] = (r, i) => r.GetDecimal(i),
                [typeof(string)] = (r, i) => r.GetString(i),
                [typeof(DateTime)] = (r, i) => r.GetDateTime(i),
                [typeof(Guid)] = (r, i) => r.GetGuid(i),
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
