using Shouldly;
using Xunit;
using System.Threading.Tasks;

namespace Badger.Data.Tests
{
    public abstract class QueryTest<T> : IClassFixture<T> where T : DbTestFixture
    {
        private readonly T fixture;
        protected readonly SessionFactory sessionFactory;

        protected QueryTest(T fixture)
        {
            this.fixture = fixture;

            this.sessionFactory = new SessionFactory(fixture.ProviderFactory, this.fixture.ConnectionString);
        }

        [Fact]
        public void ExecuteQueryTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var people = session.Execute(fixture.QueryFactory.CreateGetAllPeopleQuery());

                people.ShouldContain(p => p.Name == this.fixture.TestPerson1.Name);
                people.ShouldContain(p => p.Name == this.fixture.TestPerson2.Name);
            }
        }

        [Fact]
        public async Task ExecuteQueryAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var people = await session.ExecuteAsync(fixture.QueryFactory.CreateGetAllPeopleQuery());

                people.ShouldContain(p => p.Name == this.fixture.TestPerson1.Name 
                                       && p.Dob == this.fixture.TestPerson1.Dob
                                       && p.Height == this.fixture.TestPerson1.Height
                                       && p.Address == this.fixture.TestPerson1.Address);
                people.ShouldContain(p => p.Name == this.fixture.TestPerson2.Name 
                                       && p.Dob == this.fixture.TestPerson2.Dob
                                       && p.Height == this.fixture.TestPerson2.Height
                                       && p.Address == this.fixture.TestPerson2.Address);
            }
        }

        [Fact]
        public void ExecuteScalarTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var peopleCount = session.Execute(fixture.QueryFactory.CreateCountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var peopleCount = await session.ExecuteAsync(fixture.QueryFactory.CreateCountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        [Fact]
        public void ExecuteScalarWhenNullWithDefaultTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = session.Execute(fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

                result.ShouldBe(10);
            }
        }

        [Fact]
        public async Task ExecuteScalarWhenNullWithDefaultAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(fixture.QueryFactory.CreateNullScalarWithDefaultQuery());

                result.ShouldBe(10);
            }
        }

        [Fact]
        public void ExecuteQueryWhenNullTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = session.Execute(fixture.QueryFactory.CreateNullScalarQuery());

                result.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteQueryWhenNullAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(fixture.QueryFactory.CreateNullScalarQuery());

                result.ShouldBeNull();
            }
        }

        [Fact]
        public void ExecuteSingleTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Dob.ShouldBe(this.fixture.TestPerson1.Dob);
            }
        }

        [Fact]
        public async Task ExecuteSingleAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Dob.ShouldBe(this.fixture.TestPerson1.Dob);
            }
        }

        
        [Fact]
        public void ExecuteSingleWithNullColumTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public void ExecuteSingleWithNullColumAndDefaultValueTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAndDefaultValueAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    fixture.QueryFactory.CreateFindPersonByNameQuery(this.fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public void ExecuteSingleWhenNoRowsTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWhenNoRowsAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    fixture.QueryFactory.CreateFindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        [Fact]
        public void QuerySessionWithNoExecutionsDoesNotThrow()
        {
            this.sessionFactory.CreateQuerySession().Dispose();
        }
    }
}
