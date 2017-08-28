using System.Data;
using System.Data.Common;

namespace Badger.Data
{
    class DbTransactionSession : IDbTransactionSession
    {
        private readonly DbConnection conn;
        private readonly IsolationLevel isolationLevel;
        private DbTransaction transaction;

        public DbTransactionSession(DbConnection conn, IsolationLevel isolationLevel)
        {
            this.conn = conn;
            this.isolationLevel = isolationLevel;
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public void Dispose()
        {
            this.conn.Close();
        }

        public int ExecuteCommand(ICommand command)
        {
             if (this.conn.State == ConnectionState.Closed)
            {
                this.conn.Open();
                this.transaction = this.conn.BeginTransaction(this.isolationLevel);
            }

            var dbCommand = this.conn.CreateCommand();
            dbCommand.Transaction = this.transaction;

            var builder = new DbCommandBuilder(dbCommand);
            return command.Execute(builder);
        }

        public TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            throw new System.NotImplementedException();
        }
    }
}
