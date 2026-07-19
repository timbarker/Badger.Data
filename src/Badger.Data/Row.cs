using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data;

internal sealed class Row(DbDataReader reader) : IRow
{
    private static readonly IDictionary<Type, Func<DbDataReader, int, object>> Readers =
        new Dictionary<Type, Func<DbDataReader, int, object>>
        {
            [typeof(char)] = (r, i) => r.GetChar(i),
            [typeof(bool)] = (r, i) => r.GetBoolean(i),
            [typeof(byte)] = (r, i) => r.GetByte(i),
            [typeof(short)] = (r, i) => r.GetInt16(i),
            [typeof(int)] = (r, i) => r.GetInt32(i),
            [typeof(long)] = (r, i) => r.GetInt64(i),
            [typeof(float)] = (r, i) => r.GetFloat(i),
            [typeof(double)] = (r, i) => r.GetDouble(i),
            [typeof(decimal)] = (r, i) => r.GetDecimal(i),
            [typeof(string)] = (r, i) => r.GetString(i),
            [typeof(DateTime)] = (r, i) => r.GetDateTime(i),
            [typeof(Guid)] = (r, i) => r.GetGuid(i)
        };

    public T Get<T>(string column, T @default = default)
    {
        var ordinal = reader.GetOrdinal(column);
        var type = typeof(T);

        return !reader.IsDBNull(ordinal)
            ? (T)Readers[Nullable.GetUnderlyingType(type) ?? type].Invoke(reader, ordinal)
            : @default;
    }
}
