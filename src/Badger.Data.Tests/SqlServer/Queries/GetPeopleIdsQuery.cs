using System;
using System.Collections.Generic;

namespace Badger.Data.Tests.SqlServer.Queries;

class GetPeopleIdsQuery(params long[] ids) : IQuery<IEnumerable<long>>
{
    public IPreparedQuery<IEnumerable<long>> Prepare(IQueryBuilder queryBuilder)
    {
        return queryBuilder
            .WithSql("select p.id from people p inner join @ids i on i.id = p.id")
            .WithTableParameter("@ids", ids)
            .WithMapper(r => r.Get<long>("id"))
            .Build();
    }
}

class QueryPersonByName(Person person) : IQuery<Person>
{
    public IPreparedQuery<Person> Prepare(IQueryBuilder queryBuilder)
    {
        return queryBuilder
            .WithSql("select top 1 * from [People] where name = @person")
            .WithParameter("person", person)
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
