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
        public void QueryTestssss()
        {
            using (var session = this.sessionFactory.CreateSession())
            {
                var people = session.ExecuteQuery(new GetAllPeopleQuery());

                people.ShouldContain(p => p.Name == this.fixture.TestPerson1.Name);
                people.ShouldContain(p => p.Name == this.fixture.TestPerson2.Name);
            }
        }
    }
}
