using System;

namespace Badger.Data.Tests.Queries
{
    internal class FindPersonByNameQuery : IQuery<Person>
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
