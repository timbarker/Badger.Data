using System;

namespace Badger.Data.Tests.Queries
{
    internal class FindPersonByNameQuery : IQuery<Person>
    {
        private readonly string _name;

        public FindPersonByNameQuery(string name)
        {
            this._name = name;
        }

        public IPreparedQuery<Person> Prepare(IQueryBuilder builder)
        {
            return builder
                .WithSql("select name, dob, height, address from people where name = @name")
                .WithParameter("name", _name)
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
