using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data
{
    class DbCommandBuilder : IDbCommandBuilder, IDbQueryBuilder
    {
        private readonly DbCommand command;

        public DbCommandBuilder(DbCommand command)
        {
            this.command = command;
        }

        private DbCommandBuilder AddParameter<T>(string name, T value, int? size = null)
        {
            var parameter = this.command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            if (size.HasValue)
                parameter.Size = size.Value;
            else if (typeof(T) == typeof(string))
                parameter.Size = size ?? (value as string).Length;

            this.command.Parameters.Add(parameter);

            return this;
        }

        IDbCommandBuilder IDbCommandBuilder.WithParameter<T>(string name, T value)
        {
            return AddParameter(name, value);
        }

        IDbCommandBuilder IDbCommandBuilder.WithParameter(string name, string value, int length)
        {
           return AddParameter(name, value, length);
        }

        IDbQueryBuilder IDbQueryBuilder.WithParameter<T>(string name, T value) 
        {
            return AddParameter(name, value);
        } 

        IDbCommandBuilder IDbCommandBuilder.WithSql(string sql)
        {
            this.command.CommandText = sql;
            return this;
        }

        IDbQueryBuilder IDbQueryBuilder.WithSql(string sql)
        {
            this.command.CommandText = sql;
            return this;
        }

        public T ExecuteScalar<T>()
        {
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(Func<IDbRow, T> mapper)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Execute<T>(Func<IDbRow, T> mapper)
        {
            using (var reader = this.command.ExecuteReader())
            {
                var row = new DbRow(reader);
                while(reader.Read())
                {
                    yield return mapper.Invoke(row);
                }
            }
        }

        public int Execute()
        {
            return this.command.ExecuteNonQuery();
        }
    }
}
