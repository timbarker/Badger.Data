using System.Collections.Generic;

namespace Badger.Data.Tests.Queries
{
    public class QueryFactory
    {
        public virtual IQuery<long> CreateCountPeopleQuery()
        {
            return new CountPeopleQuery();
        }

        public virtual IQuery<Person> CreateFindPersonByNameQuery(string name)
        {
            return new FindPersonByNameQuery(name);
        }

        public virtual IQuery<IEnumerable<Person>> CreateGetAllPeopleQuery()
        {
            return new GetAllPeopleQuery();
        }

        public virtual IQuery<string> CreateNullScalarQuery()
        {
            return new NullScalarQuery();
        }

        public virtual IQuery<long> CreateNullScalarWithDefaultQuery()
        {
            return new NullScalarQueryWithDefault();
        }
    }
}
