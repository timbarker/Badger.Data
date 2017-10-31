using System.Data.SqlClient;
using Badger.Data.Tests.SqlServer.Queries;
using Dapper;

namespace Badger.Data.Tests.SqlServer
{
    public class SqlServerTestFixture : DbTestFixture
    {
        public SqlServerTestFixture() 
            : base(SqlClientFactory.Instance, new SqlServerQueryFactory())
        {
            InitTestDatabase();
        }

        public override string ConnectionString => CreateConnectionString(this.TestDatabase);

        protected override void CreateTestTables()
        {
            this.Connection.Execute(@"create table people (
                                        id bigint identity(1,1) primary key,
                                        name varchar(100) not null, 
                                        dob date not null,
                                        height int null,
                                        address varchar(100) null)");
        }


        protected override void CreateTestDatabase()
        {
            using (var conn = new SqlConnection(CreateConnectionString("master")))
            {
                conn.Execute($"create database {this.TestDatabase}");
            }
        }

        protected override void DestroyTestDatabase()
        {
            using (var conn = new SqlConnection(CreateConnectionString("master")))
            {
                conn.Execute($"drop database {this.TestDatabase}");
            }
        }

        private static string CreateConnectionString(string database)
        {
            return new SqlConnectionStringBuilder
            {
                Pooling = false,
                DataSource = "localhost",
                IntegratedSecurity = true,
                InitialCatalog = database
            }.ConnectionString;
        }
    }
}
