using System.Data;

namespace Badger.Data
{
    public interface IDbSessionFactory
    {
        IDbSession CreateSession();
        IAsyncDbSession CreateAsyncSession();
        IDbTransactionSession CreateTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IAsyncDbTransactionSession CreateAsyncTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
