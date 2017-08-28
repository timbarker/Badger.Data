using System;
using System.Data;
using System.Data.Common;

namespace Badger.Data
{
    class DbSession : IDbSession
    {
        private readonly DbConnection conn;

        public DbSession(DbConnection conn)
        {
            this.conn = conn;
        }

        public void Dispose()
        {
            this.conn.Dispose();
        }

        public int ExecuteCommand(ICommand command)
        {
            if (this.conn.State == ConnectionState.Closed)
                this.conn.Open();

            var builder = new DbCommandBuilder(this.conn.CreateCommand());
            return command.Execute(builder);
        }

        public TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            throw new NotImplementedException();
        }
    }
}