namespace Badger.Data.Tests.SqlServer.Queries
{
    internal class SqlServerCountPeopleQuery : IQuery<long>
    {
        public IPreparedQuery<long> Prepare(IQueryBuilder builder)
        {
            return builder
                .WithSql("select count_big(*) from people")
                .WithScalar<long>()
                .Build();
        }
    }
}