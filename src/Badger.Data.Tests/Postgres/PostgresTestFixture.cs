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
            CreateTestDatabase();

            OpenTestConnection();

            this.Connection.Execute(
                @"create table people(
                    id bigserial primary key, 
                    name varchar(100) not null, 
                    dob date not null)");

            this.Connection.Execute(
                "insert into people (name, dob) values (@Name, @Dob)",
                TestPerson1);

            this.Connection.Execute(
                "insert into people (name, dob) values (@Name, @Dob)",
                TestPerson2);
        }

        private void CreateTestDatabase()
        {
            using (var conn = new NpgsqlConnection(this.baseConnectionString))
            {
                conn.Execute($"create database {this.TestDatabase}");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            DropDatabase();
        }

        private void DropDatabase()
        {
            using (var conn = new NpgsqlConnection(this.baseConnectionString))
            {
                conn.Execute($"drop database {this.TestDatabase}");
            }
        }
    }
}
