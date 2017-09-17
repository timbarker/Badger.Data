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

        public IAsyncSession CreateAsyncSession()
        {
            return new AsyncSession(CreateConnection());
        }

        public IAsyncTransactionSession CreateAsyncTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new AsyncTransactionSession(CreateConnection(), isolationLevel);
        }

        public ISession CreateSession()
        {
            return new Session(CreateConnection());
        }

        public ITransactionSession CreateTransactionSession(IsolationLevel isolationLevel)
        {
            return new TransactionSession(CreateConnection(), isolationLevel);
        }

        private DbConnection CreateConnection()
        {
            var connection = this.providerFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;
            return connection;
        }
    }
}
