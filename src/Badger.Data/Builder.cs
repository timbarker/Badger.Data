using System;
using System.Data.Common;

namespace Badger.Data
{
    internal abstract class Builder<TBuilder> 
      where TBuilder : class
    {
        protected readonly DbCommand command;

        public Builder(DbCommand command)
        {
            this.command = command;
        }

        protected TBuilder AddParameter<T>(string name, T value, int? size = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Paramter name must not be null or empty", nameof(name));

            var parameter = this.command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            if (size.HasValue)
                parameter.Size = size.Value;
            else if (typeof(T) == typeof(string))
                parameter.Size = size ?? (value as string).Length;

            this.command.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder WithParameter(string name, string value, int length)
        {
           return AddParameter(name, value, length);
        }

        public TBuilder WithParameter<T>(string name, T value) 
        {
            return AddParameter(name, value);
        } 

        public TBuilder WithSql(string sql)
        {
            if (string.IsNullOrEmpty(sql)) 
                throw new ArgumentException("SQL must not be null or empty", nameof(sql));
            
            this.command.CommandText = sql;
            return this as TBuilder;
        }
    }
}
