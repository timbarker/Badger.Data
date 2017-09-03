using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Badger.Data.Tests
{
    public class QueryTest : IClassFixture<SqlLiteTestFixture>
    {
        private readonly SqlLiteTestFixture fixture;
        private readonly DbSessionFactory sessionFactory;

        public QueryTest(SqlLiteTestFixture fixture)
        {
            this.fixture = fixture;

            this.sessionFactory = new DbSessionFactory(SqliteFactory.Instance, this.fixture.ConnectionString);
        }

        [Fact]
        public void ExecuteQueryTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var people = session.ExecuteQuery(new GetAllPeopleQuery());

                people.ShouldContain(p => p.Name == this.fixture.TestPerson1.Name);
                people.ShouldContain(p => p.Name == this.fixture.TestPerson2.Name);
            }
        }

        class GetAllPeopleQuery : IQuery<IEnumerable<Person>>
        {
            public IEnumerable<Person> Execute(IDbQueryBuilder builder)
            {
                return builder
                    .WithSql("select id, name, dob from people")
                    .Execute(r => new Person 
                        { 
                            Id = r.Get<long>("id"), 
                            Name = r.Get<string>("name"), 
                            Dob = r.Get<DateTime>("dob")
                        });
            }
        }

        [Fact]
        public void ExecuteScalarTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var peopleCount = session.ExecuteQuery(new CountPeopleQuery());

                peopleCount.ShouldBe(2);
            }
        }

        class CountPeopleQuery : IQuery<long>
        {
            public long Execute(IDbQueryBuilder builder)
            {
                return builder
                    .WithSql("select count(*) from people")
                    .ExecuteScalar<long>();
            }
        }

        [Fact]
        public void ExecuteScalarWhenNullWithDefaultTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var result = session.ExecuteQuery(new NullScalarQueryWithDefault());

                result.ShouldBe(0);
            }
        }

        class NullScalarQueryWithDefault : IQuery<long>
        {
            public long Execute(IDbQueryBuilder builder)
            {
                return builder
                    .WithSql("select null")
                    .ExecuteScalar(0L);
            }
        }

        [Fact]
        public void ExecuteQueryWhenNullTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var result = session.ExecuteQuery(new NullScalarQuery());

                result.ShouldBeNull();
            }
        }

        class NullScalarQuery : IQuery<string>
        {
            public string Execute(IDbQueryBuilder builder)
            {
                return builder
                    .WithSql("select null")
                    .ExecuteScalar<string>();
            }
        }

        [Fact]
        public void ExecuteSingleTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var person = session.ExecuteQuery(
                    new FindPersonByNameQuery(this.fixture.TestPerson1.Name));

                person.Dob.ShouldBe(this.fixture.TestPerson1.Dob);
            }
        }

        [Fact]
        public void ExecuteSingleWhenNoRowsTest()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var person = session.ExecuteQuery(
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

            public Person Execute(IDbQueryBuilder builder)
            {
                return builder
                    .WithSql("select name, dob from people where name = @name")
                    .WithParameter("name", this.name)
                    .ExecuteSingle(row => new Person 
                    {
                        Name = row.Get<string>("name"),
                        Dob = row.Get<DateTime>("dob")
                    });
            }
        }
    }
}
