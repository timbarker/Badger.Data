using System.Data;

namespace Badger.Data.Postgres
{
    public interface IPostgresSessionFactory : ISessionFactory
    {
        IPostgresCommandSession CreateInsertCommandSession(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
