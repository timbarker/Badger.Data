using Dapper;
using Npgsql;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresTestFixture : DbTestFixture
    {
        private string _baseConnectionString = "Host=localhost;Username=postgres;Password=password;Pooling=false";
        public override string ConnectionString => $"{_baseConnectionString};Database={TestDatabase}";

        public PostgresTestFixture() 
            : base (NpgsqlFactory.Instance)
        {
            InitTestDatabase();
        }

        protected override void CreateTestTables()
        {
            Connection.Execute(
                @"create table people(
                    id bigserial primary key, 
                    name varchar(100) not null, 
                    dob date not null,
                    height int null,
                    address varchar(100) null)");
        }

        protected override void CreateTestDatabase()
        {
            using (var conn = new NpgsqlConnection(_baseConnectionString))
            {
                conn.Execute($"create database {TestDatabase}");
            }
        }
        
        protected override void DestroyTestDatabase()
        {
            using (var conn = new NpgsqlConnection(_baseConnectionString))
            {
                conn.Execute($"drop database {TestDatabase}");
            }
        }
    }
}
