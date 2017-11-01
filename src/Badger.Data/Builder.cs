using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Badger.Data
{
    internal abstract class Builder<TBuilder> 
      where TBuilder : class
    {
        protected readonly DbCommand Command;
        private readonly ParameterFactory _parameterFactory;

        public Builder(DbCommand command, ParameterFactory parameterFactory)
        {
            Command = command;
            _parameterFactory = parameterFactory;
        }

        public TBuilder WithParameter(string name, string value, int length)
        {
            Command.Parameters.Add(_parameterFactory.Create(name, value, length));
            return this as TBuilder;
        }

        public TBuilder WithParameter(string name, object value) 
        {
            Command.Parameters.Add(_parameterFactory.Create(name, value));
            return this as TBuilder;
        }

        public TBuilder WithTableParameter<T>(string name, IEnumerable<T> value)
        {
            Command.Parameters.Add(_parameterFactory.Create(name, value));
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
}
