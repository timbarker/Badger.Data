namespace Badger.Data.Tests.Queries
{
    internal class CountPeopleQuery : IQuery<long>
    {
        public IPreparedQuery<long> Prepare(IQueryBuilder builder)
        {
            return builder
                .WithSql("select count(*) from people")
                .WithScalar<long>()
                .Build();
        }
    }
}
