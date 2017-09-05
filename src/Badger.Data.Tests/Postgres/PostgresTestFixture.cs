using Dapper;
using System.IO;
using Npgsql;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresTestFixture : DbTestFixture
    {
        private string baseConnectionString = "Host=localhost;Username=postgres;Password=password;Pooling=false";
        public override string ConnectionString => $"{baseConnectionString};Database={this.TestDatabase}";

        public PostgresTestFixture() 
            : base (NpgsqlFactory.Instance)
        {
            InitTestDatabase();
        }

        protected override void CreateTestTables()
        {
            this.Connection.Execute(
                @"create table people(
                    id bigserial primary key, 
                    name varchar(100) not null, 
                    dob date not null)");
        }

        protected override void CreateTestDatabase()
        {
            using (var conn = new NpgsqlConnection(this.baseConnectionString))
            {
                conn.Execute($"create database {this.TestDatabase}");
            }
        }
        protected override void DestroyTestDatabase()
        {
            using (var conn = new NpgsqlConnection(this.baseConnectionString))
            {
                conn.Execute($"drop database {this.TestDatabase}");
            }
        }
    }
}
