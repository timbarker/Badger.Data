using System;
using System.Data;
using System.Data.Common;
using Badger.Data.Sessions;

namespace Badger.Data.Postgres
{
    public class PostgresSessionFactory : IPostgresSessionFactory
    {
        private readonly Config _config;
        private readonly ParameterFactory _parameterFactory;

        public static IPostgresSessionFactory With(Action<Config> configBuilder)
        {
            var config = new Config();
            configBuilder.Invoke(config);
            var sessionFactory = new PostgresSessionFactory(config);
            return sessionFactory;
        }

        private PostgresSessionFactory(Config config)
        {
            _config = config;
            _parameterFactory = new ParameterFactory(_config.ProviderFactory, _config.ParameterHandlers);
        }

        public ICommandSession CreateCommandSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new CommandSession(CreateConnection(), _parameterFactory, isolationLevel);
        }

        public IQuerySession CreateQuerySession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new QuerySession(CreateConnection(), _parameterFactory, isolationLevel);
        }

        private DbConnection CreateConnection()
        {
            var connection = _config.ProviderFactory.CreateConnection();
            connection.ConnectionString = _config.ConnectionString;
            return connection;
        }

        public IPostgresCommandSession CreateInsertCommandSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new PostgresCommandSession(CreateConnection(), _parameterFactory, isolationLevel);
        }
    }
}
