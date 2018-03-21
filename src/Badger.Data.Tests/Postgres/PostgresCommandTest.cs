using System;
using Badger.Data.Postgres;
using Dapper;
using Shouldly;
using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresCommandTest : CommandTest<PostgresTestFixture>
    {
        private readonly PostgresTestFixture _fixture;
        private readonly IPostgresSessionFactory _postgresSessionFactory;

        public PostgresCommandTest(PostgresTestFixture fixture)
            : base (fixture)
        {
            _fixture = fixture;
            _postgresSessionFactory = fixture.CreatePostgresSessionFactory();
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
                    .WithSql("insert into people(name, dob) values (@name, @dob) returning id")
                    .WithParameter("name", _name)
                    .WithParameter("dob", _dob)
                    .Build();
            }
        }

        [Fact]
        public void SessionInsertShouldAlterOneRowAndReturnGeneratedId()
        {
            var name = Guid.NewGuid().ToString();
            var dob = new DateTime(1990, 5, 20);
            long id;

            using (var session = _postgresSessionFactory.CreateInsertCommandSession())
            {
                var executionResult = session.ExecuteInsert<long>(new InsertPersonCommand(name, dob), "id");
                executionResult.RowsAffected.ShouldBe(1);
                id = executionResult.Id;
                session.Commit();
            }

            var queryResult = _fixture.Connection.QuerySingle<Person>(
                "select name, dob from people where id = @id", new { id });

            queryResult.Dob.ShouldBe(dob);
            queryResult.Name.ShouldBe(name);
        }
    }
}
