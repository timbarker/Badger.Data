using System.Data.Common;

namespace Badger.Data
{
    abstract class DbBaseBuilder<TBuilder> : IDbExecutorBuilder
      where TBuilder : class, IDbExecutorBuilder
    {
        protected readonly DbCommand command;

        public DbBaseBuilder(DbCommand command)
        {
            this.command = command;
        }

        protected TBuilder AddParameter<T>(string name, T value, int? size = null)
        {
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
            this.command.CommandText = sql;
            return this as TBuilder;
        }

        public abstract IDbExecutor Build();
    }
}
