using System.Data;

namespace Badger.Data
{
    public interface ISessionFactory
    {
        IQuerySession CreateQuerySession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        ICommandSession CreateCommandSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
