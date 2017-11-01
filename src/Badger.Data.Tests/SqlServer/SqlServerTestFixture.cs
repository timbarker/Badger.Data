using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Badger.Data.Tests.SqlServer.Queries;
using Dapper;
using Microsoft.SqlServer.Server;

namespace Badger.Data.Tests.SqlServer
{
    public class SqlServerTestFixture : DbTestFixture
    {
        public SqlServerTestFixture() 
            : base(SqlClientFactory.Instance, new SqlServerQueryFactory())
        {
            InitTestDatabase();
        }

        public override string ConnectionString => CreateConnectionString(TestDatabase);

        public override ISessionFactory CreateSessionFactory()
        {
            return SessionFactory.With(config =>
                config
                    .WithConnectionString(ConnectionString)
                    .WithProviderFactory(ProviderFactory)
                    .WithTableParameterHandler<long>((value, parameter) =>
                    {
                        var sqlParameter = (SqlParameter)parameter;

                        SqlMetaData[] tvpDefinition = { new SqlMetaData("id", SqlDbType.BigInt) };
                        sqlParameter.Value = value.Select(i =>
                        {
                            var sqlDataRecord = new SqlDataRecord(tvpDefinition);
                            sqlDataRecord.SetInt64(0, i);
                            return sqlDataRecord;
                        }).ToList();

                        sqlParameter.SqlDbType = SqlDbType.Structured;
                        sqlParameter.TypeName = "t_BigIntArray";

                    })
                    .WithParameterHandler<Person>((value, parameter) =>
                    {
                        parameter.Value = value.Name;
                        parameter.DbType = DbType.String;
                    }));
        }

        protected override void CreateTestTables()
        {
            Connection.Execute(@"create table people (
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
                conn.Execute($"create database {TestDatabase}");
            }

            using (var conn = new SqlConnection(CreateConnectionString(TestDatabase)))
            {
                conn.Execute("create type [t_BigIntArray] as table ([id] bigint)");
            }
        }

        protected override void DestroyTestDatabase()
        {
            using (var conn = new SqlConnection(CreateConnectionString("master")))
            {
                conn.Execute($"drop database {TestDatabase}");
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
