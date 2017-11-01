using System;
using System.Threading.Tasks;
using Dapper;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests
{
    public abstract class CommandTest<T> : IClassFixture<T> where T : DbTestFixture
    {
        private readonly T _fixture;
        private readonly ISessionFactory _sessionFactory;

        protected CommandTest(T fixture)
        {
            this._fixture = fixture;
            _sessionFactory = fixture.CreateSessionFactory();
        }

        class InsertPersonCommand : ICommand
        {
            private readonly string _name;
            private readonly DateTime _dob;

            public InsertPersonCommand(string name, DateTime dob)
            {
                this._name = name;
                this._dob = dob;
            }
            
            public IPreparedCommand Prepare(ICommandBuilder builder)
            {
                return builder
                    .WithSql("insert into people(name, dob) values (@name, @dob)")
                    .WithParameter("name", _name)
                    .WithParameter("dob", _dob)
                    .Build();
            }
        }

        [Fact]
        public void SessionInsertShouldAlterOneRow()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = _sessionFactory.CreateCommandSession())
            {
                session.Execute(new InsertPersonCommand(name, dob)).ShouldBe(1);
                session.Commit();
            }
        
            var result = _fixture.Connection.QuerySingle<Person>(
                "select name, dob from people where name = @name", new { name });

            result.Dob.ShouldBe(dob);
        }

        [Fact]
        public async Task SessionInsertShouldAlterOneRowAsync()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = _sessionFactory.CreateCommandSession())
            {
                (await session.ExecuteAsync(new InsertPersonCommand(name, dob))).ShouldBe(1);
                session.Commit();
            }
        
            var result = _fixture.Connection.QuerySingle<Person>(
                "select name, dob from people where name = @name", new { name });

            result.Dob.ShouldBe(dob);
        }

        [Fact]
        public void UncommitedTransactionSessionInsertShouldNotAlterAnyRows()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = _sessionFactory.CreateCommandSession())
            {
                session.Execute(new InsertPersonCommand(name, dob)).ShouldBe(1);
            }
        
            var result = _fixture.Connection.QuerySingleOrDefault<Person>(
                "select name, dob from people where name = @name", new { name });

            result.ShouldBeNull();
        }

        [Fact]
        public async Task UncommitedTransactionSessionInsertShouldNotAlterAnyRowsAsync()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);

            using (var session = _sessionFactory.CreateCommandSession())
            {
                (await session.ExecuteAsync(new InsertPersonCommand(name, dob))).ShouldBe(1);
            }
        
            var result = _fixture.Connection.QuerySingleOrDefault<Person>(
                "select name, dob from people where name = @name", new { name });

            result.ShouldBeNull();
        }

        [Fact]
        public void CommandSessionWithNoExecutionsDoesNotThrow()
        {
            _sessionFactory.CreateCommandSession().Dispose();
        }

        [Fact]
        public void CommittedCommandSessionWithNoExecutionsDoesNotThrow()
        {
            using (var session = _sessionFactory.CreateCommandSession())
            {
                session.Commit();
            }
        }
    }
}
