using System;
using System.Data;
using System.Data.Common;
using Badger.Data.Sessions;

namespace Badger.Data
{
    public class SessionFactory : ISessionFactory
    {
        private readonly Config _config;
        private readonly ParameterFactory _parameterFactory;

        public static ISessionFactory With(Action<Config> configBuilder)
        {
            var config = new Config();
            configBuilder.Invoke(config);
            var sessionFactory = new SessionFactory(config);
            return sessionFactory;
        }

        private SessionFactory(Config config)
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
    }
}
