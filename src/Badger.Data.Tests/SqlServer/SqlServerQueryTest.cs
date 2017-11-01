using Badger.Data.Tests.SqlServer.Queries;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests.SqlServer
{
    [Trait("ExcludeFromTravis", "True")]
    public class SqlServerQueryTest : QueryTest<SqlServerTestFixture>
    {
        public SqlServerQueryTest(SqlServerTestFixture fixture) 
            : base(fixture)
        {
        }

        [Fact]
        public void QueryWithCustomTableParameterTypeTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var peopleIds = new[] {1L, 2L};
                var result = session.Execute(new GetPeopleIdsQuery(peopleIds));

                result.ShouldBe(peopleIds);
            }
        }

        [Fact]
        public void QueryWithCustomParameterTypeTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            { 
                var result = session.Execute(new QueryPersonByName(_fixture.TestPerson1));

                result.Dob.ShouldBe(_fixture.TestPerson1.Dob);
            }
        }
    }
}
