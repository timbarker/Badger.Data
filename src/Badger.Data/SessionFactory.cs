using System.Data;
using System.Data.Common;
using Badger.Data.Sessions;

namespace Badger.Data
{
    public class SessionFactory : ISessionFactory
    {
        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

        public SessionFactory(DbProviderFactory providerFactory, string connectionString)
        {
            this.providerFactory = providerFactory;
            this.connectionString = connectionString;
        }

        public ICommandSession CreateCommandSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new CommandSession(CreateConnection(), isolationLevel);
        }

        public IQuerySession CreateQuerySession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new QuerySession(CreateConnection(), isolationLevel);
        }

        private DbConnection CreateConnection()
        {
            var connection = this.providerFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;
            return connection;
        }
    }
}
