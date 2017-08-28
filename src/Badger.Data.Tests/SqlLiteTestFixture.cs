using System;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Badger.Data.Tests
{
    public class SqlLiteTestFixture : IDisposable
    {
        private string dbFile;
        public string ConnectionString { get; }
        public SqliteConnection Connection { get; }
        public SqlLiteTestFixture()
        {
            this.dbFile = $"{Guid.NewGuid()}.db";
            this.ConnectionString = $"Data Source={this.dbFile}";
            this.Connection = new SqliteConnection(this.ConnectionString);
            this.Connection.Open();

            this.Connection.Execute(
                @"create table people(
                    id integer primary key autoincrement, 
                    name text not null, 
                    dob text not null)");
        }

        public void Dispose()
        {
            this.Connection.Dispose();
            File.Delete(this.dbFile);
        }
    }
}
