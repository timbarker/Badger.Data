using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;

namespace Badger.Data.Tests
{
    public abstract class QueryTest<T> : IClassFixture<T> where T : DbTestFixture
    {
        private readonly T fixture;
        private readonly SessionFactory sessionFactory;

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
                var people = session.Execute(new GetAllPeopleQuery());

                people.ShouldContain(p => p.Name == this.fixture.TestPerson1.Name);
                people.ShouldContain(p => p.Name == this.fixture.TestPerson2.Name);
            }
        }

        [Fact]
        public async Task ExecuteQueryAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var people = await session.ExecuteAsync(new GetAllPeopleQuery());

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

        class GetAllPeopleQuery : IQuery<IEnumerable<Person>>
        {
            public IPreparedQuery<IEnumerable<Person>> Prepare(IQueryBuilder builder)
            {
                return builder
                    .WithSql("select id, name, dob, height, address from people")
                    .WithMapper(r => new Person 
                        { 
                            Id = r.Get<long>("id"), 
                            Name = r.Get<string>("name"), 
                            Dob = r.Get<DateTime>("dob"),
                            Height = r.Get<int?>("height"),
                            Address = r.Get<string>("address")
                        })
                    .Build();
            }
        }

        [Fact]
        public void ExecuteScalarTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var peopleCount = session.Execute(new CountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        [Fact]
        public async Task ExecuteScalarAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var peopleCount = await session.ExecuteAsync(new CountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        class CountPeopleQuery : IQuery<long>
        {
            public IPreparedQuery<long> Prepare(IQueryBuilder builder)
            {
                return builder
                    .WithSql("select count(*) from people")
                    .WithScalar<long>()
                    .Build();
            }
        }

        [Fact]
        public void ExecuteScalarWhenNullWithDefaultTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = session.Execute(new NullScalarQueryWithDefault());

                result.ShouldBe(10);
            }
        }

        [Fact]
        public async Task ExecuteScalarWhenNullWithDefaultAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(new NullScalarQueryWithDefault());

                result.ShouldBe(10);
            }
        }

        class NullScalarQueryWithDefault : IQuery<long>
        {
            public IPreparedQuery<long> Prepare(IQueryBuilder builder)
            {
                return builder
                    .WithSql("select null")
                    .WithScalar(10L)
                    .Build();
            }
        }

        [Fact]
        public void ExecuteQueryWhenNullTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = session.Execute(new NullScalarQuery());

                result.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteQueryWhenNullAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var result = await session.ExecuteAsync(new NullScalarQuery());

                result.ShouldBeNull();
            }
        }

        class NullScalarQuery : IQuery<string>
        {
            public IPreparedQuery<string> Prepare(IQueryBuilder builder)
            {
                return builder
                    .WithSql("select null")
                    .WithScalar<string>()
                    .Build();
            }
        }

        [Fact]
        public void ExecuteSingleTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    new FindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Dob.ShouldBe(this.fixture.TestPerson1.Dob);
            }
        }

        [Fact]
        public async Task ExecuteSingleAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    new FindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Dob.ShouldBe(this.fixture.TestPerson1.Dob);
            }
        }

        
        [Fact]
        public void ExecuteSingleWithNullColumTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    new FindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    new FindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Address.ShouldBeNull();
            }
        }

        [Fact]
        public void ExecuteSingleWithNullColumAndDefaultValueTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    new FindPersonByNameQuery(this.fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public async Task ExecuteSingleWithNullColumAndDefaultValueAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    new FindPersonByNameQuery(this.fixture.TestPerson2.Name));

                person.Height.ShouldBe(-1);
            }
        }

        [Fact]
        public void ExecuteSingleWhenNoRowsTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = session.Execute(
                    new FindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ExecuteSingleWhenNoRowsAsyncTest()
        {
            using (var session = this.sessionFactory.CreateQuerySession())
            {
                var person = await session.ExecuteAsync(
                    new FindPersonByNameQuery("invalid name"));

                person.ShouldBeNull();
            }
        }

        class FindPersonByNameQuery : IQuery<Person>
        {
            private readonly string name;

            public FindPersonByNameQuery(string name)
            {
                this.name = name;
            }

            public IPreparedQuery<Person> Prepare(IQueryBuilder builder)
            {
                return builder
                    .WithSql("select name, dob, height, address from people where name = @name")
                    .WithParameter("name", this.name)
                    .WithSingleMapper(row => new Person 
                    {
                        Name = row.Get<string>("name"),
                        Dob = row.Get<DateTime>("dob"),
                        Height = row.Get<int>("height", -1),
                        Address = row.Get<string>("address")
                    })
                    .Build();
            }
        }
    }
}
