using Npgsql;
using Shouldly;
using System;
using Xunit;

namespace Badger.Data.Tests.Postgres;

[Trait("Category", "Integration")]
public class PostgresTimeoutTest(PostgresTestFixture fixture) : IClassFixture<PostgresTestFixture>
{
    private readonly ISessionFactory _sessionFactory = SessionFactory.With(config =>
        {
            config.WithConnectionString(fixture.ConnectionString)
                .WithProviderFactory(fixture.ProviderFactory);
        });

    class TimeoutQuery(int sleep, int timeout) : IQuery<int>
    {
        public IPreparedQuery<int> Prepare(IQueryBuilder queryBuilder)
        {
            return queryBuilder
                .WithSql("select pg_sleep(:sleep)")
                .WithTimeout(TimeSpan.FromSeconds(timeout))
                .WithParameter("sleep", sleep)
                .WithScalar(42)
                .Build();
        }
    }

    [Fact]
    public void ThrowsWhenQueryExceedsTimeout()
    {
        Assert.Throws<NpgsqlException>(() =>
        {
            using var session = _sessionFactory.CreateQuerySession();
            session.Execute(new TimeoutQuery(5, 1));
        });
    }

    [Fact]
    public void DoesNotThrowWhenQueryDoesntExceedTimeout()
    {
        using var session = _sessionFactory.CreateQuerySession();
        var result = session.Execute(new TimeoutQuery(1, 5));
        result.ShouldBe(42);
    }

    class TimeoutCommand : ICommand
    {
        public IPreparedCommand Prepare(ICommandBuilder commandBuilder)
        {
            return commandBuilder
                .WithSql("select pg_sleep(5)")
                .WithTimeout(TimeSpan.FromSeconds(1))
                .Build();
        }
    }

    [Fact]
    public void ThrowsWhenCommandExceedsTimeout()
    {
        Assert.Throws<NpgsqlException>(() =>
        {
            using var session = _sessionFactory.CreateCommandSession();
            session.Execute(new TimeoutCommand());
        });
    }
}
