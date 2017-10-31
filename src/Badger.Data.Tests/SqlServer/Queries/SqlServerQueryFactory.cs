using Badger.Data.Tests.Queries;

namespace Badger.Data.Tests.SqlServer.Queries
{
    internal class SqlServerQueryFactory : QueryFactory
    {
        public override IQuery<long> CreateCountPeopleQuery()
        {
            return new SqlServerCountPeopleQuery();
        }
    }
}
