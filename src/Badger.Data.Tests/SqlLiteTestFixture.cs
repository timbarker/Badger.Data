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

        public Person TestPerson1 = new Person { Name = "Bill", Dob = new DateTime(2000, 1, 1)};
        public Person TestPerson2 = new Person { Name = "Ben", Dob = new DateTime(2001, 1, 1)};


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

            this.Connection.Execute(
                "insert into people (name, dob) values (@Name, @Dob)",
                TestPerson1);

            this.Connection.Execute(
                "insert into people (name, dob) values (@Name, @Dob)",
                TestPerson2);
        }

        public void Dispose()
        {
            this.Connection.Dispose();
            File.Delete(this.dbFile);
        }
    }
}
