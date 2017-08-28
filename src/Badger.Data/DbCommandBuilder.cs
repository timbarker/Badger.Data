using System.Data.Common;

namespace Badger.Data
{
    class DbCommandBuilder : IDbCommandBuilder
    {
        private readonly DbCommand command;

        public DbCommandBuilder(DbCommand command)
        {
            this.command = command;
        }

        public IDbCommandBuilder WithParameter<T>(string name, T value)
        {
            var parameter = this.command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            this.command.Parameters.Add(parameter);

            return this;
        }

        public int Execute()
        {
            return this.command.ExecuteNonQuery();
        }

        public IDbCommandBuilder WithSql(string sql)
        {
            this.command.CommandText = sql;
            return this;
        }
    }
}
