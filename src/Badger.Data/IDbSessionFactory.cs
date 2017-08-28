using System.Data;

namespace Badger.Data
{
    public interface IDbSessionFactory
    {
        IDbSession CreateSession();
        IDbTransactionSession CreateTransactionSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
