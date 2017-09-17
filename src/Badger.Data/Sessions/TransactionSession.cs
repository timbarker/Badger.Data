using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Badger.Data.Sessions
{
    internal sealed class TransactionSession : Session, ITransactionSession
    {
        private readonly IsolationLevel isolationLevel;
        private DbTransaction transaction;

        public TransactionSession(DbConnection conn, IsolationLevel isolationLevel)
            : base(conn)
        {
            this.isolationLevel = isolationLevel;
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        protected override DbCommand CreateCommand()
        {
            var command = base.CreateCommand();
            command.Transaction = this.transaction;
            return command;
        }

        protected override void OpenConnection()
        {
            base.OpenConnection();
            this.transaction = this.conn.BeginTransaction(this.isolationLevel);
        }
    }
}
