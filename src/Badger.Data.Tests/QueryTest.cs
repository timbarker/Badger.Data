using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Badger.Data.Tests;

public abstract class QueryTest<T>(T fixture) : IClassFixture<T> where T : DbTestFixture
{
    protected readonly ISessionFactory SessionFactory = fixture.CreateSessionFactory();
    protected T Fixture { get; } = fixture;

    [Fact]
    public void ExecuteQueryTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var people = session.Execute(Fixture.QueryFactory.CreateGetAllPeopleQuery());

        people.ShouldContain(p => p.Name == Fixture.TestPerson1.Name);
        people.ShouldContain(p => p.Name == Fixture.TestPerson2.Name);
    }

    [Fact]
    public async Task ExecuteQueryAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var people = await session.ExecuteAsync(Fixture.QueryFactory.CreateGetAllPeopleQuery(), TestContext.Current.CancellationToken);

        people.ShouldContain(p => p.Name == Fixture.TestPerson1.Name
                               && p.Dob == Fixture.TestPerson1.Dob
                               && p.Height == Fixture.TestPerson1.Height
                               && p.Address == Fixture.TestPerson1.Address);
        people.ShouldContain(p => p.Name == Fixture.TestPerson2.Name
                               && p.Dob == Fixture.TestPerson2.Dob
                               && p.Height == Fixture.TestPerson2.Height
                               && p.Address == Fixture.TestPerson2.Address);
    }

    [Fact]
    public void ExecuteScalarTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var peopleCount = session.Execute(Fixture.QueryFactory.CreateCountPeopleQuery());

        peopleCount.ShouldBe(2);
    }

    [Fact]
    public async Task ExecuteScalarAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var peopleCount = await session.ExecuteAsync(Fixture.QueryFactory.CreateCountPeopleQuery(), TestContext.Current.CancellationToken);

        peopleCount.ShouldBe(2);
    }

    [Fact]
    public void ExecuteScalarWhenNullWithDefaultTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = session.Execute(Fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

        result.ShouldBe(10);
    }

    [Fact]
    public async Task ExecuteScalarWhenNullWithDefaultAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = await session.ExecuteAsync(Fixture.QueryFactory.CreateNullScalarWithDefaultQuery(), TestContext.Current.CancellationToken);

        result.ShouldBe(10);
    }

    [Fact]
    public void ExecuteQueryWhenNullTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = session.Execute(Fixture.QueryFactory.CreateNullScalarQuery());

        result.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteQueryWhenNullAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var result = await session.ExecuteAsync(Fixture.QueryFactory.CreateNullScalarQuery(), TestContext.Current.CancellationToken);

        result.ShouldBeNull();
    }

    [Fact]
    public void ExecuteSingleTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson1.Name));

        person.Dob.ShouldBe(Fixture.TestPerson1.Dob);
    }

    [Fact]
    public async Task ExecuteSingleAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson1.Name), TestContext.Current.CancellationToken);

        person.Dob.ShouldBe(Fixture.TestPerson1.Dob);
    }


    [Fact]
    public void ExecuteSingleWithNullColumTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson1.Name));

        person.Address.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteSingleWithNullColumAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson1.Name), TestContext.Current.CancellationToken);

        person.Address.ShouldBeNull();
    }

    [Fact]
    public void ExecuteSingleWithNullColumAndDefaultValueTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson2.Name));

        person.Height.ShouldBe(-1);
    }

    [Fact]
    public async Task ExecuteSingleWithNullColumAndDefaultValueAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            Fixture.QueryFactory.CreateFindPersonByNameQuery(Fixture.TestPerson2.Name), TestContext.Current.CancellationToken);

        person.Height.ShouldBe(-1);
    }

    [Fact]
    public void ExecuteSingleWhenNoRowsTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = session.Execute(
            Fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

        person.ShouldBeNull();
    }

    [Fact]
    public async Task ExecuteSingleWhenNoRowsAsyncTest()
    {
        using var session = SessionFactory.CreateQuerySession();
        var person = await session.ExecuteAsync(
            Fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"), TestContext.Current.CancellationToken);

        person.ShouldBeNull();
    }

    [Fact]
    public void QuerySessionWithNoExecutionsDoesNotThrow()
    {
        SessionFactory.CreateQuerySession().Dispose();
    }
}
