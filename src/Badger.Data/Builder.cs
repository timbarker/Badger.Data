using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data;

internal abstract class Builder<TBuilder>(DbCommand command, ParameterFactory parameterFactory)
  where TBuilder : class
{
    protected readonly DbCommand Command = command;

    public TBuilder WithParameter(string name, string value, int length)
    {
        Command.Parameters.Add(parameterFactory.Create(name, value, length));
        return this as TBuilder;
    }

    public TBuilder WithParameter<T>(string name, T value)
    {
        Command.Parameters.Add(parameterFactory.Create(name, value));
        return this as TBuilder;
    }

    public TBuilder WithTableParameter<T>(string name, IEnumerable<T> value)
    {
        Command.Parameters.Add(parameterFactory.Create(name, value));
        return this as TBuilder;
    }

    public TBuilder WithTimeout(TimeSpan timeout)
    {
        Command.CommandTimeout = (int)timeout.TotalSeconds;
        return this as TBuilder;
    }

    public TBuilder WithSql(string sql)
    {
        if (string.IsNullOrEmpty(sql))
            throw new ArgumentException("SQL must not be null or empty", nameof(sql));

        Command.CommandText = sql;
        return this as TBuilder;
    }
}
