using System.Data;

namespace Badger.Data
{
    public interface ISessionFactory
    {
        ISession CreateSession();
        IAsyncSession CreateAsyncSession();
        ITransactionSession CreateTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IAsyncTransactionSession CreateAsyncTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
