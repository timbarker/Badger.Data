using System;
using System.Collections.Generic;

namespace Badger.Data.Tests.Queries
{
    internal class GetAllPeopleQuery : IQuery<IEnumerable<Person>>
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
}
