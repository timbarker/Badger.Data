using System;
using Npgsql;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresTimeoutTest : IClassFixture<PostgresTestFixture>
    {
        readonly SessionFactory sessionFactory;

        public PostgresTimeoutTest(PostgresTestFixture fixture)
        {
            sessionFactory = new SessionFactory(fixture.ProviderFactory, fixture.ConnectionString);
        }

        class TimeoutQuery : IQuery<int>
        {
            readonly int sleep;
            readonly int timeout;

            public TimeoutQuery(int sleep, int timeout)
            {
                this.sleep = sleep;
                this.timeout = timeout;
            }

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
                using (var session = this.sessionFactory.CreateQuerySession())
                {
                    session.Execute(new TimeoutQuery(5, 1));
                }
            });
        }

        [Fact]
        public void DoesNotThrowWhenQueryDoesntExceedTimeout()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
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
                using (var session = this.sessionFactory.CreateCommandSession())
                {
                    session.Execute(new TimeoutCommand());
                }
            });
        }
    }
}
