using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Badger.Data.Queries;
using Badger.Data.Commands;

namespace Badger.Data.Sessions
{
    internal class Session : ISession
    {
        protected readonly DbConnection conn;

        public Session(DbConnection conn)
        {
            this.conn = conn;
        }

        public void Dispose()
        {
            this.conn.Dispose();
        }

        public int ExecuteCommand(ICommand command)
        {
            var builder = new CommandBuilder(CreateCommand());
            return command.Prepare(builder).Execute();
        }

        public TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            var builder = new QueryBuilder(CreateCommand());
            return query.Prepare(builder).Execute();
        }
        
        protected virtual DbCommand CreateCommand()
        {
            EnsureConnection();

            return this.conn.CreateCommand();
        }

        private void EnsureConnection()
        {
            if (this.conn.State == ConnectionState.Closed)
                OpenConnection();
        }

        protected virtual void OpenConnection()
        {
            this.conn.Open(); 
        }
    }
}
