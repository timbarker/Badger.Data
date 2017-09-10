using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Badger.Data.Queries;
using Badger.Data.Commands;

namespace Badger.Data.Sessions
{
    class DbSession : IDbSession
    {
        protected readonly DbConnection conn;

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
            var builder = new Commands.DbCommandBuilder(CreateCommand());
            return ((IDbExecutor<int>)command.Prepare(builder)).Execute();
        }

        public IEnumerable<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            var builder = new DbQueryBuilder<TResult>(CreateCommand());
            return ((IDbExecutor<IEnumerable<TResult>>)query.Prepare(builder)).Execute();
        }

        public TResult ExecuteQuery<TResult>(IScalarQuery<TResult> query)
        {
            var builder = new DbScalarQueryBuilder<TResult>(CreateCommand());
            return ((IDbExecutor<TResult>)query.Prepare(builder)).Execute();
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
