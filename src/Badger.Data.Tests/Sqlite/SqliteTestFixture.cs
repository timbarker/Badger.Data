using System;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Badger.Data.Tests.Sqlite
{
    public class SqliteTestFixture : DbTestFixture
    {
        public override string ConnectionString => $"Data Source={this.TestDatabase}";
        public SqliteTestFixture()
            : base (SqliteFactory.Instance)
        {
            OpenTestConnection();

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

        public override void Dispose()
        {
            base.Dispose();
            File.Delete(this.TestDatabase);
        }
    }
}
