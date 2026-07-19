using Badger.Data.Tests.SqlServer.Queries;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests.SqlServer;

[Trait("Category", "Integration")]
public class SqlServerQueryTest(SqlServerTestFixture fixture) : QueryTest<SqlServerTestFixture>(fixture)
{
    [Fact]
    public void QueryWithCustomTableParameterTypeTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var peopleIds = new[] { 1L, 2L };
        var result = session.Execute(new GetPeopleIdsQuery(peopleIds));

        result.ShouldBe(peopleIds);
    }

    [Fact]
    public void QueryWithCustomParameterTypeTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = session.Execute(new QueryPersonByName(Fixture.TestPerson1));

        result.Dob.ShouldBe(Fixture.TestPerson1.Dob);
    }
}
