using Dapper;
using Microsoft.Data.Sqlite;
using System.IO;

namespace Badger.Data.Tests.Sqlite
{
    public class SqliteTestFixture : DbTestFixture
    {
        public override string ConnectionString => $"Data Source={TestDatabase}";

        public SqliteTestFixture()
            : base(SqliteFactory.Instance)
        {
            InitTestDatabase();
        }

        protected override void CreateTestTables()
        {
            Connection.Execute(
                @"create table people(
                    id integer primary key autoincrement, 
                    name text not null, 
                    dob text not null,
                    height integer null,
                    address text null)");
        }

        protected override void DestroyTestDatabase()
        {
            File.Delete(TestDatabase);
        }
    }
}
