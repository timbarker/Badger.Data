using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests
{
    public abstract class QueryTest<T> : IClassFixture<T> where T : DbTestFixture
    {
        protected readonly T _fixture;
        protected readonly ISessionFactory SessionFactory;

        protected QueryTest(T fixture)
        {
            this._fixture = fixture;
            SessionFactory = fixture.CreateSessionFactory();
        }

        [Fact]
        public void ExecuteQueryTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var people = session.Execute(_fixture.QueryFactory.CreateGetAllPeopleQuery());

                people.ShouldContain(p => p.Name == _fixture.TestPerson1.Name);
                people.ShouldContain(p => p.Name == _fixture.TestPerson2.Name);
            }
        }

        [Fact]
        public async Task ExecuteQueryAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var people = await session.ExecuteAsync(_fixture.QueryFactory.CreateGetAllPeopleQuery());

                people.ShouldContain(p => p.Name == _fixture.TestPerson1.Name 
                                       && p.Dob == _fixture.TestPerson1.Dob
                                       && p.Height == _fixture.TestPerson1.Height
                                       && p.Address == _fixture.TestPerson1.Address);
                people.ShouldContain(p => p.Name == _fixture.TestPerson2.Name 
                                       && p.Dob == _fixture.TestPerson2.Dob
                                       && p.Height == _fixture.TestPerson2.Height
                                       && p.Address == _fixture.TestPerson2.Address);
            }
        }

        [Fact]
        public void ExecuteScalarTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var peopleCount = session.Execute(_fixture.QueryFactory.CreateCountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var peopleCount = await session.ExecuteAsync(_fixture.QueryFactory.CreateCountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        [Fact]
        public void ExecuteScalarWhenNullWithDefaultTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var result = session.Execute(_fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

                result.ShouldBe(10);
            }
        }

        [Fact]
        public async Task ExecuteScalarWhenNullWithDefaultAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(_fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

                result.ShouldBe(10);
            }
        }

        [Fact]
        public void ExecuteQueryWhenNullTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var result = session.Execute(_fixture.QueryFactory.CreateNullScalarQuery());

                result.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteQueryWhenNullAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(_fixture.QueryFactory.CreateNullScalarQuery());

                result.ShouldBeNull();
            }
        }

        [Fact]
        public void ExecuteSingleTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson1.Name));

                person.Dob.ShouldBe(_fixture.TestPerson1.Dob);
            }
        }

        [Fact]
        public async Task ExecuteSingleAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson1.Name));

                person.Dob.ShouldBe(_fixture.TestPerson1.Dob);
            }
        }

        
        [Fact]
        public void ExecuteSingleWithNullColumTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public void ExecuteSingleWithNullColumAndDefaultValueTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAndDefaultValueAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery(_fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public void ExecuteSingleWhenNoRowsTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWhenNoRowsAsyncTest()
        {
            using (var session = SessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    _fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        [Fact]
        public void QuerySessionWithNoExecutionsDoesNotThrow()
        {
            SessionFactory.CreateQuerySession().Dispose();
        }
    }
}
