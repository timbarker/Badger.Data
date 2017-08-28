using System.Data;
using System.Data.Common;

namespace Badger.Data
{
    public class DbSessionFactory : IDbSessionFactory
    {
        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

        public DbSessionFactory(DbProviderFactory providerFactory, string connectionString)
        {
            this.providerFactory = providerFactory;
            this.connectionString = connectionString;
        }

        public IDbSession CreateSession()
        {
            return new DbSession(CreateConnection());
        }

        public IDbTransactionSession CreateTransactionSession(IsolationLevel isolationLevel)
        {
            return new DbTransactionSession(CreateConnection(), isolationLevel);
        }

        private DbConnection CreateConnection()
        {
            var connection = this.providerFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;
            return connection;
        }
    }
}
