using Shouldly;
using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresQueryTest : QueryTest<PostgresTestFixture>
    {
        public PostgresQueryTest(PostgresTestFixture fixture) 
            : base(fixture)
        {            
        }

        class QueryWithArrayParameter : IQuery<long>
        {
            public IPreparedQuery<long> Prepare(IQueryBuilder queryBuilder)
            {
                return queryBuilder.WithSql("select count(*) from people where name = any(@names)")
                                   .WithParameter("names", new[] {"Bill"})
                                   .WithScalar<long>()
                                   .Build();
            }
        }

        [Fact]
        public void QueryWithArrayParameterTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                session.Execute(new QueryWithArrayParameter()).ShouldBe(1);
            }
        }
    }
}
