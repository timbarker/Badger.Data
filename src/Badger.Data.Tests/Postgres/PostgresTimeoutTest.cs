using System;
using Npgsql;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresTimeoutTest : IClassFixture<PostgresTestFixture>
    {
        private readonly ISessionFactory _sessionFactory;

        public PostgresTimeoutTest(PostgresTestFixture fixture)
        {
            _sessionFactory = SessionFactory.With(config =>
            {
                config.WithConnectionString(fixture.ConnectionString)
                    .WithProviderFactory(fixture.ProviderFactory);
            });
        }

        class TimeoutQuery : IQuery<int>
        {
            readonly int _sleep;
            readonly int _timeout;

            public TimeoutQuery(int sleep, int timeout)
            {
                this._sleep = sleep;
                this._timeout = timeout;
            }

            public IPreparedQuery<int> Prepare(IQueryBuilder queryBuilder)
            {
                return queryBuilder
                    .WithSql("select pg_sleep(:sleep)")
                    .WithTimeout(TimeSpan.FromSeconds(_timeout))
                    .WithParameter("sleep", _sleep)
                    .WithScalar(42)
                    .Build();
            }
        }

        [Fact]
        public void ThrowsWhenQueryExceedsTimeout()
        {
            Assert.Throws<NpgsqlException>(() =>
            {
                using (var session = _sessionFactory.CreateQuerySession())
                {
                    session.Execute(new TimeoutQuery(5, 1));
                }
            });
        }

        [Fact]
        public void DoesNotThrowWhenQueryDoesntExceedTimeout()
        {
            using (var session = _sessionFactory.CreateQuerySession())
            {
                var result = session.Execute(new TimeoutQuery(1, 5));
                result.ShouldBe(42);
            }
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
                using (var session = _sessionFactory.CreateCommandSession())
                {
                    session.Execute(new TimeoutCommand());
                }
            });
        }
    }
}
