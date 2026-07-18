using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Badger.Data.Tests;

public abstract class QueryTest<T>(T fixture) : IClassFixture<T> where T : DbTestFixture
{
    protected readonly ISessionFactory SessionFactory = fixture.CreateSessionFactory();

    [Fact]
    public void ExecuteQueryTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var people = session.Execute(fixture.QueryFactory.CreateGetAllPeopleQuery());

        people.ShouldContain(p => p.Name == fixture.TestPerson1.Name);
        people.ShouldContain(p => p.Name == fixture.TestPerson2.Name);
    }

    [Fact]
    public async Task ExecuteQueryAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var people = await session.ExecuteAsync(fixture.QueryFactory.CreateGetAllPeopleQuery(), TestContext.Current.CancellationToken);

        people.ShouldContain(p => p.Name == fixture.TestPerson1.Name
                               && p.Dob == fixture.TestPerson1.Dob
                               && p.Height == fixture.TestPerson1.Height
                               && p.Address == fixture.TestPerson1.Address);
        people.ShouldContain(p => p.Name == fixture.TestPerson2.Name
                               && p.Dob == fixture.TestPerson2.Dob
                               && p.Height == fixture.TestPerson2.Height
                               && p.Address == fixture.TestPerson2.Address);
    }

    [Fact]
    public void ExecuteScalarTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var peopleCount = session.Execute(fixture.QueryFactory.CreateCountPeopleQuery());

        peopleCount.ShouldBe(2);
    }

    [Fact]
    public async Task ExecuteScalarAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var peopleCount = await session.ExecuteAsync(fixture.QueryFactory.CreateCountPeopleQuery(), TestContext.Current.CancellationToken);

        peopleCount.ShouldBe(2);
    }

    [Fact]
    public void ExecuteScalarWhenNullWithDefaultTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = session.Execute(fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

        result.ShouldBe(10);
    }

    [Fact]
    public async Task ExecuteScalarWhenNullWithDefaultAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = await session.ExecuteAsync(fixture.QueryFactory.CreateNullScalarWithDefaultQuery(), TestContext.Current.CancellationToken);

        result.ShouldBe(10);
    }

    [Fact]
    public void ExecuteQueryWhenNullTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = session.Execute(fixture.QueryFactory.CreateNullScalarQuery());

        result.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteQueryWhenNullAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = await session.ExecuteAsync(fixture.QueryFactory.CreateNullScalarQuery(), TestContext.Current.CancellationToken);

        result.ShouldBeNull();
    }

    [Fact]
    public void ExecuteSingleTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson1.Name));

        person.Dob.ShouldBe(fixture.TestPerson1.Dob);
    }

    [Fact]
    public async Task ExecuteSingleAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson1.Name), TestContext.Current.CancellationToken);

        person.Dob.ShouldBe(fixture.TestPerson1.Dob);
    }


    [Fact]
    public void ExecuteSingleWithNullColumTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson1.Name));

        person.Address.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteSingleWithNullColumAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson1.Name), TestContext.Current.CancellationToken);

        person.Address.ShouldBeNull();
    }

    [Fact]
    public void ExecuteSingleWithNullColumAndDefaultValueTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson2.Name));

        person.Height.ShouldBe(-1);
    }

    [Fact]
    public async Task ExecuteSingleWithNullColumAndDefaultValueAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            fixture.QueryFactory.CreateFindPersonByNameQuery(fixture.TestPerson2.Name), TestContext.Current.CancellationToken);

        person.Height.ShouldBe(-1);
    }

    [Fact]
    public void ExecuteSingleWhenNoRowsTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

        person.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteSingleWhenNoRowsAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"), TestContext.Current.CancellationToken);

        person.ShouldBeNull();
    }

    [Fact]
    public void QuerySessionWithNoExecutionsDoesNotThrow()
    {
        SessionFactory.CreateQuerySession().Dispose();
    }
}
