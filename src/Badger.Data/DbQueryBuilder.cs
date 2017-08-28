using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Badger.Data
{
    class DbQueryBuilder : IDbQueryBuilder
    {
        private DbCommand command;

        public DbQueryBuilder(DbCommand dbCommand)
        {
            this.command = dbCommand;
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

        public T ExecuteScalar<T>()
        {
            throw new NotImplementedException();
        }

        public T ExecuteSingle<T>(Func<IDbRow, T> mapper)
        {
            throw new NotImplementedException();
        }

        public IDbQueryBuilder WithParameter<T>(string name, T value)
        {
            var parameter = this.command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            this.command.Parameters.Add(parameter);

            return this;
        }

        public IDbQueryBuilder WithSql(string sql)
        {
            this.command.CommandText = sql;
            return this;
        }
    }
}
