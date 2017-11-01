namespace Badger.Data.Tests.Queries
{
    internal class NullScalarQueryWithDefault : IQuery<long>
    {
        public IPreparedQuery<long> Prepare(IQueryBuilder builder)
        {
            return builder
                .WithSql("select null")
                .WithScalar(10L)
                .Build();
        }
    }
}
