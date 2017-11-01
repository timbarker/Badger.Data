using System;
using System.Collections.Generic;

namespace Badger.Data.Tests.SqlServer.Queries
{
    class GetPeopleIdsQuery : IQuery<IEnumerable<long>>
    {
        private readonly long[] _ids;

        public GetPeopleIdsQuery(params long[] ids)
        {
            _ids = ids;
        }

        public IPreparedQuery<IEnumerable<long>> Prepare(IQueryBuilder queryBuilder)
        {
            return queryBuilder
                .WithSql("select p.id from people p inner join @ids i on i.id = p.id")
                .WithTableParameter("@ids", _ids)
                .WithMapper(r => r.Get<long>("id"))
                .Build();
        }
    }

    class QueryPersonByName : IQuery<Person>
    {
        private readonly Person _person;

        public QueryPersonByName(Person person)
        {
            _person = person;
        }

        public IPreparedQuery<Person> Prepare(IQueryBuilder queryBuilder)
        {
            return queryBuilder
                .WithSql("select top 1 * from [People] where name = @person")
                .WithParameter("person", _person)
                .WithSingleMapper(row => new Person
                {
                    Name = row.Get<string>("name"),
                    Dob = row.Get<DateTime>("dob"),
                    Height = row.Get("height", -1),
                    Address = row.Get<string>("address")
                })
                .Build();
        }
    }
}
